namespace Shoplify.Services.Models.Conversation
{
    using System;

    using Shoplify.Domain;

    public class ConversationServiceModel
    {
        public string Id { get; set; }

        public string BuyerId { get; set; }

        public string SellerId { get; set; }

        public string AdvertisementId { get; set; }

        public DateTime StartedOn { get; set; }

        public bool IsReadByBuyer { get; set; }

        public bool IsReadBySeller { get; set; }

        public bool IsArchivedByBuyer { get; set; }

        public bool IsArchivedBySeller { get; set; }
    }
}
