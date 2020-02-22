using System.Collections.Generic;
using Shoplify.Common;

namespace Shoplify.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetByCategory(string categoryId, int page = 1)
        {
            if (page <= 0)
            {
                return Redirect("/Home/Index");
            }

            var ads = await advertisementService.GetByCategoryIdAsync(categoryId, page, GlobalConstants.AdsOnPageCount);

            var result = new List<ListingViewModel>();

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

                result.Add(adViewModel);
            }

            return View("Listing", result);
        }

        [Authorize]
        public async Task<IActionResult> GetBySearch(string search, int page = 1)
        {
            if (page <= 0)
            {
                return Redirect("/Home/Index");
            }

            var ads = await advertisementService.GetBySearchAsync(search, page, GlobalConstants.AdsOnPageCount);

            var result = new List<ListingViewModel>();

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

                result.Add(adViewModel);
            }

            return View("Listing", result);
        }
    }
}
