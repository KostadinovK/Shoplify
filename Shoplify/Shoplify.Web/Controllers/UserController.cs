namespace Shoplify.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Common;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models;
    using Shoplify.Web.ViewModels.Advertisement;
    using Shoplify.Web.ViewModels.User;

    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class UserController : Controller
    {
        private readonly IAdvertisementService advertisementService;
        private readonly IUserAdWishlistService userAdWishlistService;
        private readonly ICategoryService categoryService;
        private readonly ISubCategoryService subCategoryService;
        private readonly ITownService townService;
        private readonly UserManager<User> userManager;
        private readonly IUserService userService;

        public UserController(
            IAdvertisementService advertisementService,
            IUserAdWishlistService userAdWishlistService,
            ICategoryService categoryService,
            ISubCategoryService subCategoryService,
            ITownService townService,
            UserManager<User> userManager,
            IUserService userService)
        {
            this.advertisementService = advertisementService;
            this.userAdWishlistService = userAdWishlistService;
            this.categoryService = categoryService;
            this.subCategoryService = subCategoryService;
            this.townService = townService;
            this.userManager = userManager;
            this.userService = userService;
        }

        public async Task<IActionResult> AddToWishlist(string adId)
        {
            if (!advertisementService.Contains(adId))
            {
                return Redirect("/Home/Index");
            }

            var ad = await advertisementService.GetByIdAsync(adId);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == ad.UserId)
            {
                return Redirect($"/Advertisement/Details?id={adId}");
            }

            await userAdWishlistService.AddToWishlistAsync(userId, adId);

            return Redirect($"/Advertisement/Details?id={adId}");
        }

        public async Task<IActionResult> RemoveFromWishlist(string adId)
        {
            if (!advertisementService.Contains(adId))
            {
                return Redirect("/Home/Index");
            }

            var ad = await advertisementService.GetByIdAsync(adId);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == ad.UserId)
            {
                return Redirect($"/Advertisement/Details?id={adId}");
            }

            await userAdWishlistService.RemoveFromWishlistAsync(userId, adId);

            return Redirect($"/Advertisement/Details?id={adId}");
        }

        private async Task<ProfileViewModel> GetProfileViewModelAsync(User user, int lastPage, int adsCount, string orderBy = "dateDesc", int page = 1)
        {

            var ads = await advertisementService.GetByUserIdAsync(user.Id, page, GlobalConstants.AdsOnPageCount, orderBy);

            ads = OrderAds(ads, orderBy);

            var viewModel = new ProfileViewModel
            {
                UserId = user.Id,
                Username = user.UserName,
                CurrentPage = page,
                LastPage = lastPage,
                TotalAdsCount = adsCount,
                OrderParam = "orderBy=" + orderBy,
                PageParam = "id=" + user.Id,
                Advertisements = new List<ListingViewModel>(),
                IsFollowedByLoggedInUser = await userService.IsFollowedByUser(User.FindFirstValue(ClaimTypes.NameIdentifier), user.Id)
            };

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
                    Image = ad.Images.FirstOrDefault()
                };

                viewModel.Advertisements.Add(adViewModel);
            }

            return viewModel;
        }

        private async Task<LoggedInProfileViewModel> GetLoggedInUserProfileViewModelAsync(User user, int lastPage, int adsCount, string orderBy = "dateDesc", int page = 1)
        {

            var ads = await advertisementService.GetByUserIdAsync(user.Id, page, GlobalConstants.AdsOnPageCount, orderBy);

            ads = OrderAds(ads, orderBy);

            var viewModel = new LoggedInProfileViewModel()
            {
                Username = user.UserName,
                CurrentPage = page,
                LastPage = lastPage,
                TotalAdsCount = adsCount,
                PageParam = "id=" + user.Id,
            };

            foreach (var ad in ads)
            {
                var category = await categoryService.GetByIdAsync(ad.CategoryId);

                string subCategoryName = null;

                if (await subCategoryService.ContainsByIdAsync(ad.SubCategoryId))
                {
                    var subCategory = await subCategoryService.GetByIdAsync(ad.SubCategoryId);
                    subCategoryName = subCategory.Name;
                }

                var adViewModel = new UserAdListingViewModel()
                {
                    CategoryName = category.Name,
                    CreatedOn = ad.CreatedOn.ToString(GlobalConstants.DateTimeFormat),
                    Id = ad.Id,
                    Name = ad.Name,
                    Price = ad.Price,
                    SubCategoryName = subCategoryName,
                    Views = ad.Views,
                    ArchivedOn = ad.ArchivedOn.GetValueOrDefault().ToString(GlobalConstants.DateTimeFormat),
                    IsPromoted = ad.IsPromoted,
                    PromotedUntil = ad.PromotedUntil.GetValueOrDefault().ToString(GlobalConstants.DateTimeFormat)
                };

                viewModel.Advertisements.Add(adViewModel);
            }

            return viewModel;
        }

        public async Task<IActionResult> Profile(string id, string orderBy = "dateDesc", int page = 1)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return Redirect("/Home/Index");
            }

            if (page <= 0)
            {
                return Redirect("/Home/Index");
            }

            var adsCount = await advertisementService.GetCountByUserIdAsync(user.Id);
            var lastPage = adsCount / GlobalConstants.AdsOnPageCount + 1;

            if (page > lastPage)
            {
                return Redirect("/Home/Index");
            }

            if (user.Id == loggedInUserId)
            {
                var loggedInUserViewModel = await GetLoggedInUserProfileViewModelAsync(user, lastPage, adsCount, orderBy, page);

                return View("LoggedInUserProfile", loggedInUserViewModel);
            }

            var viewModel = await GetProfileViewModelAsync(user, lastPage, adsCount, orderBy, page);

            return View(viewModel);
        }

        public async Task<IActionResult> Follow(string userId)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Redirect("/Home/Index");
            }

            if (!await userService.IsFollowedByUser(loggedInUserId, userId))
            {
                await userService.FollowUserAsync(loggedInUserId, userId);
            }

            return Redirect($"/User/Profile?id={userId}");
        }

        public async Task<IActionResult> Unfollow(string userId)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Redirect("/Home/Index");
            }

            if (await userService.IsFollowedByUser(loggedInUserId, userId))
            {
                await userService.UnfollowUserAsync(loggedInUserId, userId);
            }

            return Redirect($"/User/Profile?id={userId}");
        }

        private IEnumerable<AdvertisementViewServiceModel> OrderAds(IEnumerable<AdvertisementViewServiceModel> ads, string orderBy)
        {
            if (orderBy == "priceAsc")
            {
                return ads.OrderBy(a => a.Price);
            }
            else if (orderBy == "priceDesc")
            {
                return ads.OrderByDescending(a => a.Price);
            }
            else if (orderBy == "dateAsc")
            {
                return ads.OrderBy(a => a.CreatedOn);
            }
            else
            {
                return ads.OrderByDescending(a => a.CreatedOn);
            }
        }
    }
}
