namespace Shoplify.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class Notification
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.NotificationTextMaxLength)]
        public string Text { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
    }
}
