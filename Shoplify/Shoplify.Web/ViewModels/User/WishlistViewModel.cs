namespace Shoplify.Web.ViewModels.User
{
    using System.Collections.Generic;

    using Shoplify.Web.ViewModels.Advertisement;

    public class WishlistViewModel
    {
        public int TotalAdsCount { get; set; }

        public int CurrentPage { get; set; }

        public int LastPage { get; set; }

        public List<WishlistAdViewModel> Advertisements { get; set; } = new List<WishlistAdViewModel>();
    }
}
