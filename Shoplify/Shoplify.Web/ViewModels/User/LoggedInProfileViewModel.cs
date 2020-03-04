namespace Shoplify.Web.ViewModels.User
{
    using System.Collections.Generic;

    using Shoplify.Web.ViewModels.Advertisement;

    public class LoggedInProfileViewModel
    {
        public string Username { get; set; }

        public string PageParam { get; set; }

        public int TotalAdsCount { get; set; }

        public int CurrentPage { get; set; }

        public int LastPage { get; set; }

        public List<UserAdListingViewModel> Advertisements { get; set; } = new List<UserAdListingViewModel>();
    }
}
