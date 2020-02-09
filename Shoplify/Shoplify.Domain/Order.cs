namespace Shoplify.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Common;
    using Enums;

    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string SellerId { get; set; }

        public User Seller { get; set; }

        [Required]
        public string BuyerId { get; set; }

        public User Buyer { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.OrderUserFirstNameMaxLength)]
        public string BuyerFirstName { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.OrderUserLastNameMaxLength)]
        public string BuyerLastName { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.OrderUserFirstNameMaxLength)]
        public string SellerFirstName { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.OrderUserLastNameMaxLength)]
        public string SellerLastName { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.OrderAddressMaxLength)]
        public string BuyerAddress { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.OrderAddressMaxLength)]
        public string SellerAddress { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public string CourierId { get; set; }

        public Courier Courier { get; set; }

        [Required]
        [Range(typeof(decimal), AttributesConstraints.OrderMinPrice, AttributesConstraints.OrderMaxPrice)]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.OrderDescriptionMaxLength)]
        public string Description { get; set; }

        public DateTime? EstimatedDelivery { get; set; }

        [Required]
        public OrderStatus Status { get; set; }
    }
}
