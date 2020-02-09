namespace Shoplify.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(AttributesConstraints.CategoryNameMaxLength)]
        public string Name { get; set; }

        public string CssIconClass { get; set; }

        public ICollection<Advertisement> Advertisements { get; set; } = new HashSet<Advertisement>();

        public ICollection<SubCategory> SubCategories { get; set; } = new HashSet<SubCategory>();
    }
}
