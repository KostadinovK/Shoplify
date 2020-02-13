namespace Shoplify.Web.BindingModels.Advertisement
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using Shoplify.Common;
    using Shoplify.Domain.Enums;

    public class CreateAdvertisementBindingModel
    {
        [Required]
        [StringLength(AttributesConstraints.AdvertisementNameMaxLength, MinimumLength = AttributesConstraints.AdvertisementNameMinLength, ErrorMessage = AttributesErrorMessages.AdvertisementTitleInvalidLength)]
        public string Name { get; set; }

        [Required]
        [Range(typeof(decimal), AttributesConstraints.AdvertisementMinPrice, AttributesConstraints.AdvertisementMaxPrice)]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.AdvertisementDescriptionMaxLength)]
        public string Description { get; set; }

        public ProductCondition Condition { get; set; }

        [Required]
        public string CategoryId { get; set; }

        [Required]
        public string SubCategoryId { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? EditedOn { get; set; }

        public bool IsArchived { get; set; }

        public DateTime? ArchivedOn { get; set; }

        [Required]
        public string TownId { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.AdvertisementAddressMaxLength)]
        public string Address { get; set; }

        [MaxLength(AttributesConstraints.AdvertisementNumberMaxLength)]
        public string Number { get; set; }

        [DataType(DataType.Upload)]
        public ICollection<IFormFile> Images { get; set; }
    }
}
