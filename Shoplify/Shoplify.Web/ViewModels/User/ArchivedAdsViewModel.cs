namespace Shoplify.Web.ViewModels.User
{
    using System.Collections.Generic;
    using Shoplify.Web.ViewModels.Advertisement;

    public class ArchivedAdsViewModel
    {
        public int TotalAdsCount { get; set; }

        public int CurrentPage { get; set; }

        public int LastPage { get; set; }

        public List<ArchivedAdViewModel> Advertisements { get; set; } = new List<ArchivedAdViewModel>();
    }
}
