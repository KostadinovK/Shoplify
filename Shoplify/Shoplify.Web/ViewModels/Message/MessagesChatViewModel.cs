namespace Shoplify.Web.ViewModels.Message
{
    using System.Collections.Generic;

    public class MessagesChatViewModel
    {
        public string AdName { get; set; }

        public string AdId { get; set; }

        public List<MessageViewModel> Messages { get; set; } = new List<MessageViewModel>();

        public string OnMessageSendSenderId { get; set; }

        public string OnMessageSendReceiverId { get; set; }

        public string ConversationId { get; set; }
    }
}
