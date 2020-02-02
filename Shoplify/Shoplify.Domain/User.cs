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
    }
}
