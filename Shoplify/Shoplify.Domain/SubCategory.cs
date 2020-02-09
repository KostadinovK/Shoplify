namespace Shoplify.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class SubCategory
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string CategoryId { get; set; }

        public Category Category { get; set; }

        [Required]
        [StringLength(AttributesConstraints.CategoryNameMaxLength)]
        public string Name { get; set; }

        public ICollection<Advertisement> Advertisements { get; set; } = new HashSet<Advertisement>();
    }
}
