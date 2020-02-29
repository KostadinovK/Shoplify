namespace Shoplify.Services.Models.Advertisement
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Microsoft.AspNetCore.Http;
    using Shoplify.Domain.Enums;

    public class AdvertisementEditServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        [DataType(DataType.Upload)]
        public ICollection<IFormFile> Images { get; set; }

        public ProductCondition Condition { get; set; }

        public string CategoryId { get; set; }

        public string SubCategoryId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? EditedOn { get; set; }

        public bool IsArchived { get; set; }

        public DateTime? ArchivedOn { get; set; }

        public string TownId { get; set; }

        public string Address { get; set; }

        public string Number { get; set; }

        public bool IsReported { get; set; }

        public DateTime? ReportedOn { get; set; }
    }
}
