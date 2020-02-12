namespace Shoplify.Domain
{
    using System;
    using System.Collections.Generic;

    public class Town
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public ICollection<Advertisement> Advertisements { get; set; } = new HashSet<Advertisement>();
    }
}
