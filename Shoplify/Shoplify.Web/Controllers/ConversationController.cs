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

        public async Task<IActionResult> Create(string buyerId, string sellerId, string adId)
        {
            if (await conversationService.ConversationExistsAsync(buyerId, sellerId, adId))
            {
                var id = await conversationService.GetIdAsync(buyerId, sellerId, adId);

                return Redirect($"/Message/Chat?conversationId={id}");
            }

            var conversation = await conversationService.CreateConversationAsync(buyerId, sellerId, adId);

            return Redirect($"/Message/Chat?conversationId={conversation.Id}");
        }

        public async Task<IActionResult> GetAllCount(string userId)
        {
            var conversationsCount = await conversationService.GetAllUnReadByUserIdCountAsync(userId);

            return Json(conversationsCount);
        }

        public async Task<IActionResult> All()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var conversations = await conversationService.GetAllByUserIdAsync(userId);

            var viewModel = new List<ConversationViewModel>();

            foreach (var conversation in conversations)
            {
                var ad = await advertisementService.GetByIdAsync(conversation.AdvertisementId);

                var buyer = await userManager.FindByIdAsync(conversation.BuyerId);
                var seller = await userManager.FindByIdAsync(conversation.SellerId);

                var conversationViewModel = new ConversationViewModel
                {
                    Id = conversation.Id,
                    BuyerId = conversation.BuyerId,
                    SellerId = conversation.SellerId,
                    BuyerName = buyer.UserName,
                    SellerName = seller.UserName,
                    AdvertisementId = ad.Id,
                    AdvertisementName = ad.Name,
                    StartedOn = conversation.StartedOn.ToString(GlobalConstants.DateTimeFormat)
                };

                if (ad.UserId == userId && userId == conversation.SellerId)
                {
                    conversationViewModel.IsRead = conversation.IsReadBySeller;
                }else
                {
                    conversationViewModel.IsRead = conversation.IsReadByBuyer;
                }

                viewModel.Add(conversationViewModel);
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Archive(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await conversationService.ArchiveAsync(id, userId);

            return Redirect("All");
        }

        public async Task<IActionResult> ArchiveAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await conversationService.ArchiveAllAsync(userId);

            return Redirect("All");
        }
    }
}
