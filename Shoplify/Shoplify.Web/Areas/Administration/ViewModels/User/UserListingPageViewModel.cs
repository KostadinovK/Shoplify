namespace Shoplify.Web.Areas.Administration.ViewModels.User
{
    using System.Collections.Generic;

    public class UserListingPageViewModel
    {
        public ICollection<UserViewModel> Users { get; set; } = new List<UserViewModel>();

        public string OrderParam { get; set; }

        public int TotalUsersCount { get; set; }

        public int CurrentPage { get; set; }

        public int LastPage { get; set; }
    }
}
