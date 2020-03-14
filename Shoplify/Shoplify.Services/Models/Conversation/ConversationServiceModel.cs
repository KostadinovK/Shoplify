namespace Shoplify.Services.Models.Conversation
{
    using System;

    using Shoplify.Domain;

    public class ConversationServiceModel
    {
        public string Id { get; set; }

        public string FirstUserId { get; set; }

        public string SecondUserId { get; set; }

        public User FirstUser { get; set; }

        public User SecondUser { get; set; }

        public string AdvertisementId { get; set; }

        public Domain.Advertisement Advertisement { get; set; }

        public DateTime StartedOn { get; set; }
    }
}
