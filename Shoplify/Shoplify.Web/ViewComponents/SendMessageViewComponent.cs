namespace Shoplify.Web.ViewComponents
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Domain;
    using Shoplify.Web.BindingModels.Message;

    public class SendMessageViewComponent : ViewComponent
    {
        private readonly UserManager<User> userManager;

        public SendMessageViewComponent(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke(string receiverId, string senderId, string conversationId)
        {
            var inputModel = new MessageBindingModel
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                ConversationId = conversationId
            };

            return View(inputModel);
        }
    }
}
