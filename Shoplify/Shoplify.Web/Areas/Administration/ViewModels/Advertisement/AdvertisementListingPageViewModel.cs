namespace Shoplify.Web.Areas.Administration.ViewModels.Advertisement
{
    using System.Collections.Generic;

    public class AdvertisementListingPageViewModel
    {
        public ICollection<AdvertisementViewModel> Ads { get; set; } = new List<AdvertisementViewModel>();

        public int TotalAdsCount { get; set; }

        public int CurrentPage { get; set; }

        public int LastPage { get; set; }
    }
}
