namespace Shoplify.Web.ViewModels.CategoryAndSubCategory
{
    using System.Collections.Generic;

    using Shoplify.Web.ViewModels.Category;
    using Shoplify.Web.ViewModels.SubCategory;

    public class ListingViewModel
    {
        public Dictionary<CategoryViewModel, List<SubCategoryViewModel>> CategoriesWithSubCategories { get; set; } = new Dictionary<CategoryViewModel, List<SubCategoryViewModel>>();
    }
}
