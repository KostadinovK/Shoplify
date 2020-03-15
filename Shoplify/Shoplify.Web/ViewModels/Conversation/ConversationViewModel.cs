namespace Shoplify.Web.ViewModels.Conversation
{
    public class ConversationViewModel
    {
        public string Id { get; set; }

        public string AdvertisementId { get; set; }

        public string AdvertisementName { get; set; }

        public string StartedOn { get; set; }

        public string FirstUserId { get; set; }

        public string SecondUserId { get; set; }

        public string FirstUserName { get; set; }

        public string SecondUserName { get; set; }

        public bool IsRead { get; set; }
    }
}
