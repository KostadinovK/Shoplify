namespace Shoplify.Web.ViewModels.Conversation
{
    public class ConversationViewModel
    {
        public string Id { get; set; }

        public string AdvertisementId { get; set; }

        public string AdvertisementName { get; set; }

        public string StartedOn { get; set; }

        public string BuyerId { get; set; }

        public string SellerId { get; set; }

        public string BuyerName { get; set; }

        public string SellerName { get; set; }

        public bool IsRead { get; set; }
    }
}
