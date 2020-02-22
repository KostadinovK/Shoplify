namespace Shoplify.Web.ViewComponents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.ViewModels.Category;
    using Shoplify.Web.ViewModels.CategoryAndSubCategory;
    using Shoplify.Web.ViewModels.SubCategory;

    public class LastAddedAdsViewComponent : ViewComponent
    {
        private readonly IAdvertisementService advertisementService;
        private readonly ICategoryService categoryService;
        private readonly ISubCategoryService subCategoryService;
        private readonly ITownService townService;
        private readonly UserManager<User> userManager;

        public LastAddedAdsViewComponent(IAdvertisementService advertisementService, UserManager<User> userManager, ICategoryService categoryService, ISubCategoryService subCategoryService, ITownService townService)
        {
            this.advertisementService = advertisementService;
            this.userManager = userManager;
            this.categoryService = categoryService;
            this.subCategoryService = subCategoryService;
            this.townService = townService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = userManager.GetUserId(HttpContext.User);

            var ads = await advertisementService.GetLatestAsync(8, userId);

            var result = new List<ViewModels.Advertisement.ListingViewModel>();

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

                var adViewModel = new ViewModels.Advertisement.ListingViewModel
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

            return View(result);
        }
    }
}
