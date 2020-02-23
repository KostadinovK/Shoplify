namespace Shoplify.Web.ViewModels.Advertisement
{
    using System.Collections.Generic;

    public class ListingPageViewModel
    {
        public ICollection<ListingViewModel> Advertisements { get; set; } = new List<ListingViewModel>();

        public string PageParam { get; set; }

        public string GetByParam { get; set; }

        public int TotalAdsCount { get; set; }
    }
}
