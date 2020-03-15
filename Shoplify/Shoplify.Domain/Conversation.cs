namespace Shoplify.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Conversation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string BuyerId { get; set; }

        [Required]
        public string SellerId { get; set; }

        [Required]
        public DateTime StartedOn { get; set; }

        public bool IsReadByBuyer{ get; set; }

        public bool IsReadBySeller { get; set; }

        public bool IsArchivedByBuyer { get; set; }

        public bool IsArchivedBySeller { get; set; }

        [Required]
        public string AdvertisementId { get; set; }

        public Advertisement Advertisement { get; set; }

        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}
