namespace Shoplify.Services.Models.Message
{
    using System;
    using Shoplify.Domain;

    public class MessageServiceModel
    {
        public string Id { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public User Sender { get; set; }

        public User Receiver { get; set; }

        public string ConversationId { get; set; }

        public Domain.Conversation Conversation { get; set; }

        public string Text { get; set; }

        public DateTime SendOn { get; set; }
    }
}
