﻿namespace Shoplify.Web.ViewModels.Advertisement
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Category;
    using Microsoft.AspNetCore.Http;
    using Shoplify.Common;
    using Shoplify.Domain.Enums;
    using Shoplify.Web.ViewModels.Town;

    public class CreateViewModel
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
        public string TownId { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.AdvertisementAddressMaxLength)]
        public string Address { get; set; }

        [Phone]
        [MaxLength(AttributesConstraints.AdvertisementNumberMaxLength)]
        public string Number { get; set; }

        [DataType(DataType.Upload)]
        public ICollection<IFormFile> Images { get; set; }

        public IEnumerable<CategoryDropdownViewModel> Categories { get; set; }

        public IEnumerable<TownDropdownViewModel> Towns { get; set; }
    }
}
