namespace Shoplify.Web.ViewModels.Advertisement
{
    using System.Collections.Generic;

    public class ListingPageViewModel
    {
        public ICollection<ListingViewModel> Advertisements { get; set; } = new List<ListingViewModel>();

        public Shoplify.Web.ViewModels.CategoryAndSubCategory.ListingViewModel CategoiesAndSubCategories { get; set; } = new Shoplify.Web.ViewModels.CategoryAndSubCategory.ListingViewModel();

        public string PageParam { get; set; }

        public string GetByParam { get; set; }

        public int TotalAdsCount { get; set; }

        public int CurrentPage { get; set; }

        public int LastPage { get; set; }
    }
}
