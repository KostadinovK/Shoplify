namespace Shoplify.Domain
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }

        public DateTime RegisteredOn { get; set; }

        public bool IsBanned { get; set; }

        public DateTime? BannedOn { get; set; }

        public ICollection<IdentityUserRole<string>> Roles { get; set; } = new HashSet<IdentityUserRole<string>>();

        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();

        public ICollection<Advertisement> Advertisements { get; set; } = new HashSet<Advertisement>();

        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();

        public ICollection<UserNotification> Notifications { get; set; } = new HashSet<UserNotification>();

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public ICollection<FollowerFollowing> Followers { get; set; } = new HashSet<FollowerFollowing>();

        public ICollection<FollowerFollowing> Followings { get; set; } = new HashSet<FollowerFollowing>();
    }
}
