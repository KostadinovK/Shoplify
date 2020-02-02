namespace Shoplify.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class Courier
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.CourierFirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.CourierLastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public DateTime HiredOn { get; set; }

        public string CurrentOrderId { get; set; }

        public Order CurrentOrder { get; set; }
    }
}
