namespace Shoplify.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models.Comment;
    using Shoplify.Web.BindingModels.Comment;

    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService commentService;
        private readonly INotificationService notificationService;
        private readonly IAdvertisementService advertisementService;
        private readonly UserManager<User> userManager;

        public CommentController(ICommentService commentService, UserManager<User> userManager, INotificationService notificationService, IAdvertisementService advertisementService)
        {
            this.commentService = commentService;
            this.userManager = userManager;
            this.notificationService = notificationService;
            this.advertisementService = advertisementService;
        }

        public async Task<IActionResult> GetAllByAd(string id)
        {
            var comments = await commentService.GetAllByAdIdAsync(id);

            return Json(comments);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateBindingModel input)
        {
            if (!ModelState.IsValid)
            {
                return Redirect($"/Advertisement/Details?id={input.AdvertisementId}");
            }

            var serviceModel = new CreateServiceModel
            {
                Text = input.Text,
                AdvertisementId = input.AdvertisementId,
                WrittenOn = DateTime.UtcNow,
                UserId = userManager.GetUserAsync(HttpContext.User).GetAwaiter().GetResult().Id
            };

            var comment = await commentService.PostAsync(serviceModel);

            var commentOwner = await userManager.FindByIdAsync(comment.UserId);
            var commentUserName = commentOwner.UserName;

            var ad = await advertisementService.GetByIdAsync(input.AdvertisementId);
            var adOwner = await userManager.FindByIdAsync(ad.UserId);

            if (adOwner.Id != commentOwner.Id)
            {
                var notificationText = $"{commentUserName} just commented on your ad: {comment.Text}";
                var notificationActionLink = $"/Advertisement/Details?id={input.AdvertisementId}";

                var notification = await notificationService.CreateNotificationAsync(notificationText, notificationActionLink);

                await notificationService.AssignNotificationToUserAsync(notification.Id, adOwner.Id);
            }

            return Redirect($"/Advertisement/Details?id={input.AdvertisementId}");
        }
    }
}
