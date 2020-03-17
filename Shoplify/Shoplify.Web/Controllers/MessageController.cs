namespace Shoplify.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Shoplify.Common;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.BindingModels.Message;
    using Shoplify.Web.Hubs;
    using Shoplify.Web.ViewModels.Message;

    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMessageService messageService;
        private readonly IConversationService conversationService;
        private readonly IAdvertisementService adService;
        private readonly UserManager<User> userManager;
        private readonly IHubContext<MessageHub> hubContext;

        public MessageController(IMessageService messageService, IConversationService conversationService, IAdvertisementService adService, UserManager<User> userManager, IHubContext<MessageHub> hubContext)
        {
            this.messageService = messageService;
            this.conversationService = conversationService;
            this.adService = adService;
            this.userManager = userManager;
            this.hubContext = hubContext;
        }

        public async Task<IActionResult> Chat(string conversationId)
        {
            var conversation = await conversationService.GetByIdAsync(conversationId);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await conversationService.MarkConversationAsReadAsync(conversation.Id, userId);

            var ad = await adService.GetByIdAsync(conversation.AdvertisementId);

            var messages = await messageService.GetAllInConversationAsync(conversationId);

            var onMessageSendReceiverId = "";

            if (conversation.BuyerId == userId)
            {
                onMessageSendReceiverId = conversation.SellerId;
            }else
            {
                onMessageSendReceiverId = conversation.BuyerId;
            }

            var viewModel = new MessagesChatViewModel
            {
                AdId = ad.Id,
                AdName = ad.Name,
                ConversationId = conversation.Id,
                OnMessageSendSenderId = userId,
                OnMessageSendReceiverId = onMessageSendReceiverId
            };

            foreach (var message in messages)
            {
                var sender = await userManager.FindByIdAsync(message.SenderId);

                viewModel.Messages.Add(new MessageViewModel
                {
                    SenderName = sender.UserName,
                    SendOn = message.SendOn.ToLocalTime().ToString(GlobalConstants.DateTimeFormat),
                    Text = message.Text
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(MessageBindingModel inputModel)
        {
            var message = await messageService.CreateMessageAsync(inputModel.ConversationId, inputModel.SenderId,
                inputModel.ReceiverId, inputModel.Text);

            var sender = await userManager.FindByIdAsync(message.SenderId);

            var messageViewModel = new MessageViewModel
            {
                Text = message.Text,
                SenderName = sender.UserName,
                SendOn = message.SendOn.ToLocalTime().ToString(GlobalConstants.DateTimeFormat)
            };

            await hubContext.Clients.User(inputModel.ReceiverId)
                .SendAsync("SendMessage", messageViewModel);

            return Json(message);
        }
    }
}
