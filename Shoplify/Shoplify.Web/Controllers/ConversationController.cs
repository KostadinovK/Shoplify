namespace Shoplify.Web.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Common;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.ViewModels.Conversation;

    public class ConversationController : Controller
    {
        private IConversationService conversationService;
        private IAdvertisementService advertisementService;
        private UserManager<User> userManager;

        public ConversationController(IConversationService conversationService, IAdvertisementService advertisementService, UserManager<User> userManager)
        {
            this.conversationService = conversationService;
            this.advertisementService = advertisementService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Create(string firstUserId, string secondUserId, string adId)
        {
            if (await conversationService.ConversationExistsAsync(firstUserId, secondUserId, adId))
            {
                var id = await conversationService.GetIdAsync(firstUserId, secondUserId, adId);

                return Redirect($"/Messages/Chat?conversationId={id}");
            }

            var conversation = await conversationService.CreateConversationAsync(firstUserId, secondUserId, adId);

            return Redirect($"/Messages/Chat?conversationId={conversation.Id}");
        }

        public async Task<IActionResult> All()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var conversations = await conversationService.GetAllByUserIdAsync(userId);

            var viewModel = new List<ConversationViewModel>();

            foreach (var conversation in conversations)
            {
                var ad = await advertisementService.GetByIdAsync(conversation.AdvertisementId);

                var firstUser = await userManager.FindByIdAsync(conversation.FirstUserId);
                var secondUser = await userManager.FindByIdAsync(conversation.SecondUserId);

                var conversationViewModel = new ConversationViewModel
                {
                    Id = conversation.Id,
                    FirstUserId = conversation.FirstUserId,
                    SecondUserId = conversation.SecondUserId,
                    FirstUserName = firstUser.UserName,
                    SecondUserName = secondUser.UserName,
                    AdvertisementId = ad.Id,
                    AdvertisementName = ad.Name,
                    StartedOn = conversation.StartedOn.ToLocalTime().ToString(GlobalConstants.DateTimeFormat)
                };

                if (ad.UserId == conversation.FirstUserId)
                {
                    conversationViewModel.IsRead = conversation.IsReadByFirstUser;
                }else
                {
                    conversationViewModel.IsRead = conversation.IsReadBySecondUser;
                }

                viewModel.Add(conversationViewModel);
            }

            return View(viewModel);
        }
    }
}
