namespace Shoplify.Web.ViewModels.User
{
    using System.Collections.Generic;

    using Shoplify.Web.ViewModels.Advertisement;

    public class ProfileViewModel
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public bool IsFollowedByLoggedInUser { get; set; }

        public string PageParam { get; set; }

        public string OrderParam { get; set; }

        public int TotalAdsCount { get; set; }

        public int CurrentPage { get; set; }

        public int LastPage { get; set; }

        public ICollection<ListingViewModel> Advertisements { get; set; } = new List<ListingViewModel>();
    }
}
