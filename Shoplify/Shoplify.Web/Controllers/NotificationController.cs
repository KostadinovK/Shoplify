using Shoplify.Common;

namespace Shoplify.Web.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.ViewModels.Notification;

    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public async Task<IActionResult> GetAllCount(string userId)
        {
            var notificationsCount = await notificationService.GetAllUnReadByUserIdCountAsync(userId);

            return Json(notificationsCount);
        }

        public async Task<IActionResult> All()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notifications = await notificationService.GetAllUnReadByUserIdAsync(userId);

            var viewModel = new List<NotificationViewModel>();

            foreach (var notification in notifications)
            {
                viewModel.Add(new NotificationViewModel
                {
                    ActionLink = notification.ActionLink,
                    Id = notification.Id,
                    Text = notification.Text,
                    CreatedOn = notification.CreatedOn.ToString(GlobalConstants.DateTimeFormat)
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Mark(string userId, string nId)
        {
            await notificationService.MarkNotificationAsReadAsync(nId, userId);

            return Redirect($"/Notification/All?userId={userId}");
        }

        public async Task<IActionResult> MarkAll(string userId)
        {
            await notificationService.MarkAllNotificationsAsReadAsync(userId);

            return Redirect($"/Notification/All?userId={userId}");
        }
    }
}
