namespace Shoplify.Services.Models.Conversation
{
    using System;

    using Shoplify.Domain;

    public class ConversationServiceModel
    {
        public string Id { get; set; }

        public string FirstUserId { get; set; }

        public string SecondUserId { get; set; }

        public string AdvertisementId { get; set; }

        public DateTime StartedOn { get; set; }

        public bool IsReadByFirstUser { get; set; }

        public bool IsReadBySecondUser { get; set; }

        public bool IsArchivedByFirstUser { get; set; }

        public bool IsArchivedBySecondUser { get; set; }
    }
}
