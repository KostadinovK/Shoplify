using System;
using System.Collections.Generic;
using System.Text;
using Shoplify.Domain.Enums;

namespace Shoplify.Services.Models
{
    public class AdvertisementViewServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public List<string> Images { get; set; }

        public ProductCondition Condition { get; set; }

        public string CategoryId { get; set; }

        public string SubCategoryId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string TownId { get; set; }

        public string Address { get; set; }

        public string Number { get; set; }
    }
}
