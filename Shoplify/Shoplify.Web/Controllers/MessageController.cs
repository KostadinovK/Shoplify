namespace Shoplify.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Shoplify.Common;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
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

            var viewModel = new MessagesChatViewModel
            {
                AdId = ad.Id,
                AdName = ad.Name
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
    }
}
