namespace Shoplify.Web.ViewModels.User
{
    using System.Collections.Generic;

    using Shoplify.Web.ViewModels.Advertisement;

    public class BannedAdsViewModel
    {
        public int TotalAdsCount { get; set; }

        public int CurrentPage { get; set; }

        public int LastPage { get; set; }

        public List<BannedAdViewModel> Advertisements { get; set; } = new List<BannedAdViewModel>();
    }
}
