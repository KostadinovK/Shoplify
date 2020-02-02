namespace Shoplify.Domain
{
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class Category
    {
        public string Id { get; set; }

        [Required]
        [StringLength(AttributesConstraints.CategoryNameMaxLength)]
        public string Name { get; set; }

        public string ImageUrl { get; set; }
    }
}
