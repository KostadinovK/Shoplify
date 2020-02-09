namespace Shoplify.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class Notification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(AttributesConstraints.NotificationTextMaxLength)]
        public string Text { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public ICollection<UserNotification> Users { get; set; } = new HashSet<UserNotification>();
    }
}
