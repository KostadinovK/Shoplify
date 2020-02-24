using Microsoft.EntityFrameworkCore;
using Shoplify.Web.ViewModels.Category;
using Shoplify.Web.ViewModels.SubCategory;

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
    using Shoplify.Web.BindingModels.Advertisement;
    using Shoplify.Web.ViewModels.Advertisement;

    [AutoValidateAntiforgeryToken]
    public class AdvertisementController : Controller
    {
        private IAdvertisementService advertisementService;
        private ICategoryService categoryService;
        private ISubCategoryService subCategoryService;
        private ITownService townService;
        private readonly UserManager<User> userManager;

        public AdvertisementController(IAdvertisementService advertisementService, ICategoryService categoryService, ISubCategoryService subCategoryService, ITownService townService, UserManager<User> userManager)
        {
            this.advertisementService = advertisementService;
            this.categoryService = categoryService;
            this.subCategoryService = subCategoryService;
            this.townService = townService;
            this.userManager = userManager;
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
                Images = advertisement.Images
            };

            await advertisementService.CreateAsync(advertisementServiceModel);

            return Redirect("/Home/Index");
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
                    Image = ad.Images.FirstOrDefault()
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
                    CreatedOn = ad.CreatedOn.ToString("dd/MM/yyyy hh:mm tt"),
                    Id = ad.Id,
                    Name = ad.Name,
                    Price = ad.Price,
                    SubCategoryName = subCategoryName,
                    TownName = town.Name,
                    Image = ad.Images.FirstOrDefault()
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
        public Task<IActionResult> Details(string id)
        {

            
            return View();
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
