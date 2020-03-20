namespace Shoplify.Web.Areas.Administration.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string RegisteredOn { get; set; }

        public string BannedOn { get; set; }

        public bool IsBanned { get; set; }
    }
}
