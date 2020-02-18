using System.Collections.Generic;
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

            viewModel.CategoriesWithSubCategories.Add(new CategoryViewModel { Name = "Home", Id = "test", CssIconClass = "fas fa-home"},
                new List<SubCategoryViewModel>() {new SubCategoryViewModel{ Id = "test", Name = "Household"}, new SubCategoryViewModel{Id = "test2", Name = "Cleaning"}});

            viewModel.CategoriesWithSubCategories.Add(new CategoryViewModel { Name = "Electronics", Id = "test2", CssIconClass = "fas fa-mobile-alt" },
                new List<SubCategoryViewModel>() { new SubCategoryViewModel { Id = "test", Name = "Laptops" }, new SubCategoryViewModel { Id = "Phones", Name = "Phones" } });

            viewModel.CategoriesWithSubCategories.Add(new CategoryViewModel { Name = "Electronics", Id = "test2", CssIconClass = "fas fa-mobile-alt" },
                new List<SubCategoryViewModel>() { new SubCategoryViewModel { Id = "test", Name = "Laptops" }, new SubCategoryViewModel { Id = "Phones", Name = "Phones" } });

          





            return View(viewModel);
        }
    }
}
