namespace Shoplify.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Shoplify.Common;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models;
    using Shoplify.Services.Models.Advertisement;
    using Shoplify.Web.BindingModels.Advertisement;
    using Shoplify.Web.ViewModels.Advertisement;
    using Shoplify.Web.ViewModels.Category;
    using Shoplify.Web.ViewModels.SubCategory;
    using Stripe;

    [AutoValidateAntiforgeryToken]
    public class AdvertisementController : Controller
    {
        private IAdvertisementService advertisementService;
        private ICategoryService categoryService;
        private ISubCategoryService subCategoryService;
        private ITownService townService;
        private IUserAdWishlistService userAdWishlistService;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly INotificationService notificationService;
        private readonly IUserService userService;

        public AdvertisementController(IAdvertisementService advertisementService, ICategoryService categoryService, ISubCategoryService subCategoryService, ITownService townService, IUserAdWishlistService userAdWishlistService, UserManager<User> userManager, IConfiguration configuration, INotificationService notificationService, IUserService userService)
        {
            this.advertisementService = advertisementService;
            this.categoryService = categoryService;
            this.subCategoryService = subCategoryService;
            this.townService = townService;
            this.userManager = userManager;
            this.userAdWishlistService = userAdWishlistService;
            this.configuration = configuration;
            this.notificationService = notificationService;
            this.userService = userService;
        }

        [Authorize]
        public IActionResult Create()
        {
            return View(new CreateBindingModel());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateBindingModel advertisement)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/Advertisement/Create");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var advertisementServiceModel = new AdvertisementCreateServiceModel
            {
                Name = advertisement.Name,
                Price = advertisement.Price,
                Description = advertisement.Description,
                Condition = advertisement.Condition,
                CategoryId = advertisement.CategoryId,
                SubCategoryId = advertisement.SubCategoryId,
                TownId = advertisement.TownId,
                Address = advertisement.Address,
                Number = advertisement.Number,
                UserId = userId,
                Images = advertisement.Images,
            };

            await advertisementService.CreateAsync(advertisementServiceModel);

            await NotifyOnAdCreateAsync(advertisementServiceModel);

            return Redirect("/Home/Index");
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ad = await advertisementService.GetByIdAsync(id);

            if (ad.UserId != userId)
            {
                return Redirect($"/Advertisement/Details?id={id}");
            }

            var modelForView = new EditBindingModel()
            {
                Id = ad.Id,
                Name = ad.Name,
                Address = ad.Address,
                CategoryId = ad.CategoryId,
                Condition = ad.Condition,
                Description = ad.Description,
                Number = ad.Number,
                Price = ad.Price,
                SubCategoryId = ad.SubCategoryId,
                TownId = ad.TownId,
                UserId = ad.UserId
            };

            return View(modelForView);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditBindingModel advertisement)
        {
            if (!ModelState.IsValid)
            {
                return Redirect($"/Advertisement/Edit?id={advertisement.Id}");
            }

            var advertisementServiceModel = new AdvertisementEditServiceModel()
            {
                Id = advertisement.Id,
                Name = advertisement.Name,
                Price = advertisement.Price,
                Description = advertisement.Description,
                Condition = advertisement.Condition,
                CategoryId = advertisement.CategoryId,
                SubCategoryId = advertisement.SubCategoryId,
                TownId = advertisement.TownId,
                Address = advertisement.Address,
                Number = advertisement.Number,
                UserId = advertisement.UserId,
                Images = advertisement.Images
            };

            await advertisementService.EditAsync(advertisementServiceModel);

            await NotifyOnAdEditAsync(advertisement.Id);

            return Redirect($"/Advertisement/Details?id={advertisement.Id}");
        }

        [Authorize]
        public async Task<IActionResult> GetByCategory(string categoryId, string orderBy = "dateDesc", int page = 1)
        {
            if (page <= 0)
            {
                return Redirect("/Home/Index");
            }

            var adsCount = await advertisementService.GetCountByCategoryIdAsync(categoryId);
            var lastPage = adsCount / GlobalConstants.AdsOnPageCount + 1;

            if (adsCount % GlobalConstants.AdsOnPageCount == 0 && adsCount > 0)
            {
                lastPage -= 1;
            }

            if (page > lastPage)
            {
                return Redirect("/Home/Index");
            }

            var ads = await advertisementService.GetByCategoryIdAsync(categoryId, page, GlobalConstants.AdsOnPageCount, orderBy);

            ads = OrderAds(ads, orderBy);

            var result = new ListingPageViewModel();

            result = await GetCategoriesAndSubCategoriesAsync(result);

            foreach (var ad in ads)
            {
                var category = await categoryService.GetByIdAsync(ad.CategoryId);

                string subCategoryName = null;

                if (await subCategoryService.ContainsByIdAsync(ad.SubCategoryId))
                {
                    var subCategory = await subCategoryService.GetByIdAsync(ad.SubCategoryId);
                    subCategoryName = subCategory.Name;
                }

                var town = await townService.GetByIdAsync(ad.TownId);

                var adViewModel = new ListingViewModel
                {
                    Address = ad.Address,
                    CategoryName = category.Name,
                    CreatedOn = ad.CreatedOn.ToString("dd/MM/yyyy hh:mm tt"),
                    Id = ad.Id,
                    Name = ad.Name,
                    Price = ad.Price,
                    SubCategoryName = subCategoryName,
                    TownName = town.Name,
                    Image = ad.Images.FirstOrDefault(),
                    IsPromoted = ad.IsPromoted,
                };

                result.Advertisements.Add(adViewModel);
            }

            result.CurrentPage = page;
            result.LastPage = lastPage;
            result.TotalAdsCount = adsCount;
            result.GetByParam = "GetByCategory";
            result.OrderParam = "orderBy=" + orderBy;
            result.PageParam = "categoryId=" + categoryId;

            return View("Listing", result);
        }

        [Authorize]
        public async Task<IActionResult> GetBySearch(string search, string orderBy = "dateDesc", int page = 1)
        {
            if (page <= 0)
            {
                return Redirect("/Home/Index");
            }

            var adsCount = await advertisementService.GetCountBySearchAsync(search);
            var lastPage = adsCount / GlobalConstants.AdsOnPageCount + 1;

            if (adsCount % GlobalConstants.AdsOnPageCount == 0 && adsCount > 0)
            {
                lastPage -= 1;
            }

            if (page > lastPage)
            {
                return Redirect("/Home/Index");
            }

            var ads = await advertisementService.GetBySearchAsync(search, page, GlobalConstants.AdsOnPageCount, orderBy);
            ads = OrderAds(ads, orderBy);

            var result = new ListingPageViewModel();

            result = await GetCategoriesAndSubCategoriesAsync(result);

            foreach (var ad in ads)
            {
                var category = await categoryService.GetByIdAsync(ad.CategoryId);

                string subCategoryName = null;

                if (await subCategoryService.ContainsByIdAsync(ad.SubCategoryId))
                {
                    var subCategory = await subCategoryService.GetByIdAsync(ad.SubCategoryId);
                    subCategoryName = subCategory.Name;
                }

                var town = await townService.GetByIdAsync(ad.TownId);

                var adViewModel = new ListingViewModel
                {
                    Address = ad.Address,
                    CategoryName = category.Name,
                    CreatedOn = ad.CreatedOn.ToString(GlobalConstants.DateTimeFormat),
                    Id = ad.Id,
                    Name = ad.Name,
                    Price = ad.Price,
                    SubCategoryName = subCategoryName,
                    TownName = town.Name,
                    Image = ad.Images.FirstOrDefault(),
                    IsPromoted = ad.IsPromoted,
                };

                result.Advertisements.Add(adViewModel);
            }

            result.CurrentPage = page;
            result.LastPage = lastPage;
            result.TotalAdsCount = adsCount;
            result.OrderParam = "orderBy=" + orderBy;
            result.GetByParam = "GetBySearch";
            result.PageParam = "search=" + search;

            return View("Listing", result);
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            if (!advertisementService.Contains(id))
            {
                return Redirect("/Home/Index");
            }

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ad = await advertisementService.GetByIdAsync(id);
            var user = await userManager.FindByIdAsync(ad.UserId);
            var town = await townService.GetByIdAsync(ad.TownId);

            if (user.Id != loggedInUserId)
            {
                await advertisementService.IncrementViewsAsync(id);
            }

            var category = await categoryService.GetByIdAsync(ad.CategoryId);

            string subCategoryName = null;

            if (await subCategoryService.ContainsByIdAsync(ad.SubCategoryId))
            {
                var subCategory = await subCategoryService.GetByIdAsync(ad.SubCategoryId);
                subCategoryName = subCategory.Name;
            }

            var viewModel = new DetailsViewModel()
            {
                Id = ad.Id,
                Address = ad.Address,
                CategoryName = category.Name,
                CategoryId = category.Id,
                SubCategoryId = ad.SubCategoryId,
                SubCategoryName = subCategoryName,
                CreatedOn = ad.CreatedOn.ToString(GlobalConstants.DateTimeFormat),
                Description = ad.Description,
                UserId = ad.UserId,
                Username = user.UserName,
                Name = ad.Name,
                Phone = ad.Number,
                TownName = town.Name,
                Price = ad.Price,
                Images = ad.Images,
                Condition = ad.Condition.ToString(),
                IsAdInLoggedUserWishlist = await userAdWishlistService.IsAdInWishlistAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), ad.Id)
            };

            ViewData["loggedUserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Archive(string id)
        {
            if (!advertisementService.Contains(id))
            {
                return Redirect("/Home/Index");
            }

            await advertisementService.ArchiveByIdAsync(id);

            return Redirect("/Home/Index");
        }

        [Authorize]
        public IActionResult Promote(string id)
        {
            if (!advertisementService.Contains(id))
            {
                return Redirect("/Home/Index");
            }

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(new PromoteViewModel{ Id = id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Promote(PromoteBindingModel input)
        {
            if (!ModelState.IsValid)
            {
                return Redirect($"/Advertisement/Promote?id={input.Id}");
            }

            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
            
            var prices = new Dictionary<string, int>(){
                {"1", 200},
                {"7", 600},
                {"14", 1000},
                {"30", 2000}
            };

            var service = new PaymentIntentService();
            var options = new PaymentIntentCreateOptions
            {
                //amount is in cents
                Amount = prices[input.PromotedDays],
                Currency = "usd",
                // Verify your integration in this guide by including this parameter
                Metadata = new Dictionary<String, String>()
                {
                    {"integration_check", "accept_a_payment"}
                }
            };
            var payment = service.Create(options);

            await advertisementService.PromoteByIdAsync(input.Id, int.Parse(input.PromotedDays));

            return Redirect($"/User/Profile?page=1");
        }

        private async Task NotifyOnAdCreateAsync(AdvertisementCreateServiceModel ad)
        {
            var adOwner = await userManager.FindByIdAsync(ad.UserId);

            var notificationText = $"{adOwner.UserName} added a new Ad: {ad.Name}";
            var notificationActionLink = $"/User/Profile?id={ad.UserId}&orderBy='dateDesc'&page=1";

            var userIds = await userService.GetAllUserIdsThatAreFollowingUserAsync(ad.UserId);

            if (userIds.Count() != 0)
            {
                var notification = await notificationService.CreateNotificationAsync(notificationText, notificationActionLink);

                await notificationService.AssignNotificationToUsersAsync(notification.Id, userIds.ToList());
            }
        }

        private async Task NotifyOnAdEditAsync(string adId)
        {
            var ad = await advertisementService.GetByIdAsync(adId);

            var adOwner = await userManager.FindByIdAsync(ad.UserId);

            var notificationText = $"{adOwner.UserName} edited one of his adds: {ad.Name}";
            var notificationActionLink = $"/Advertisement/Details?id={ad.Id}";

            var userIds = await userService.GetAllUserIdsThatAreFollowingUserAsync(ad.UserId);

            if (userIds.Count() != 0)
            {
                var notification = await notificationService.CreateNotificationAsync(notificationText, notificationActionLink);

                await notificationService.AssignNotificationToUsersAsync(notification.Id, userIds.ToList());
            }
        }

        private IEnumerable<AdvertisementViewServiceModel> OrderAds(IEnumerable<AdvertisementViewServiceModel> ads, string orderBy)
        {
            if (orderBy == "priceAsc")
            {
                return ads.OrderByDescending(a => a.IsPromoted).ThenBy(a => a.Price);
            }
            else if (orderBy == "priceDesc")
            {
                return ads.OrderByDescending(a => a.IsPromoted).ThenByDescending(a => a.Price);
            }
            else if (orderBy == "dateAsc")
            {
                return ads.OrderByDescending(a => a.IsPromoted).ThenBy(a => a.CreatedOn);
            }
            else
            {
                return ads.OrderByDescending(a => a.IsPromoted).ThenByDescending(a => a.CreatedOn);
            }
        }

        private async Task<ListingPageViewModel> GetCategoriesAndSubCategoriesAsync(ListingPageViewModel result)
        {
            var categories = categoryService.GetAll();

            foreach (var category in categories)
            {
                var subCategories = await subCategoryService
                    .GetAllByCategoryId(category.Id)
                    .OrderBy(s => s.Name)
                    .Select(s => new SubCategoryViewModel
                    {
                        Name = s.Name,
                        Id = s.Id
                    })
                    .ToListAsync();

                result.CategoiesAndSubCategories.CategoriesWithSubCategories
                    .Add(new CategoryViewModel { Name = category.Name, Id = category.Id, CssIconClass = category.CssIconClass }, subCategories);
            }

            return result;
        }

    }
}
