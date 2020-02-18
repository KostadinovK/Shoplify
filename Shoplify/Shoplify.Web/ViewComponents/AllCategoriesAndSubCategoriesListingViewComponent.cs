using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Shoplify.Web.ViewModels.Category;
using Shoplify.Web.ViewModels.CategoryAndSubCategory;
using Shoplify.Web.ViewModels.SubCategory;

namespace Shoplify.Web.ViewComponents
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Services.Interfaces;

    [ViewComponent(Name = "AllCategoriesAndSubCategoriesListing")]
    public class AllCategoriesAndSubCategoriesListingViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;
        private readonly ISubCategoryService subCategoryService;

        public AllCategoriesAndSubCategoriesListingViewComponent(ICategoryService categoryService, ISubCategoryService subCategoryService)
        {
            this.categoryService = categoryService;
            this.subCategoryService = subCategoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModel = new ListingViewModel
            {
                CategoriesWithSubCategories = new Dictionary<CategoryViewModel, List<SubCategoryViewModel>>()
            };

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

                viewModel.CategoriesWithSubCategories
                    .Add(new CategoryViewModel { Name = category.Name, Id = category.Id, CssIconClass = category.CssIconClass }, subCategories);
            }

            return View(viewModel);
        }
    }
}
