namespace Shoplify.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class Category
    {
        public string Id { get; set; }

        [Required]
        [StringLength(AttributesConstraints.CategoryNameMaxLength)]
        public string Name { get; set; }

        public string CssIconClass { get; set; }

        public ICollection<Advertisement> Advertisements { get; set; } = new HashSet<Advertisement>();
    }
}
