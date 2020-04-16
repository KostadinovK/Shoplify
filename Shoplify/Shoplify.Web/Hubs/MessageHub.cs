namespace Shoplify.Web.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using Shoplify.Common;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.BindingModels.Message;
    using Shoplify.Web.ViewModels.Message;

    public class MessageHub : Hub
    {
        private readonly IMessageService messageService;
        private readonly UserManager<User> userManager;

        public MessageHub(IMessageService messageService, UserManager<User> userManager)
        {
            this.messageService = messageService;
            this.userManager = userManager;
        }

        public async Task SendMessage(MessageBindingModel inputModel)
        {
            var messageServiceModel = await messageService.CreateMessageAsync(inputModel.ConversationId, inputModel.SenderId, inputModel.ReceiverId, inputModel.Text);

            var sender = await userManager.FindByIdAsync(messageServiceModel.SenderId);

            var messageViewModel = new MessageViewModel
            {
                SendOn = messageServiceModel.SendOn.ToString(GlobalConstants.DateTimeFormat),
                SenderName = sender.UserName,
                Text = messageServiceModel.Text
            };

            await Clients.Users(inputModel.ReceiverId)
                .SendAsync("SendMessage", messageViewModel);
        }
    }
}
