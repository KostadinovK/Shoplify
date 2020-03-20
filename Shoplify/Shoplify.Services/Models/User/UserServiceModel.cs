namespace Shoplify.Services.Models.User
{
    using System;

    public class UserServiceModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public DateTime RegisteredOn { get; set; }

        public bool IsBanned { get; set; }

        public DateTime BannedOn { get; set; }
    }
}
