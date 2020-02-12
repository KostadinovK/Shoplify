namespace Shoplify.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common;
    using Enums;

    public class Advertisement
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(AttributesConstraints.AdvertisementNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [Range(typeof(decimal), AttributesConstraints.AdvertisementMinPrice, AttributesConstraints.AdvertisementMaxPrice)]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.AdvertisementDescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public string Images { get; set; }

        public ProductCondition Condition { get; set; }

        [Required]
        public string CategoryId { get; set; }

        public Category Category { get; set; }

        [Required]
        public string SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? EditedOn { get; set; }

        public bool IsArchived { get; set; }

        public DateTime? ArchivedOn { get; set; }

        [Required]
        public string TownId { get; set; }

        public Town Town { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.AdvertisementAddressMaxLength)]
        public string Address { get; set; }

        [MaxLength(AttributesConstraints.AdvertisementNumberMaxLength)]
        public string Number { get; set; }

        public bool IsReported { get; set; }

        public DateTime? ReportedOn { get; set; }

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();
    }
}
