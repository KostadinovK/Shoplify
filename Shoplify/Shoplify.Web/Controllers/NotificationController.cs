using System.Collections.Generic;
using System.Linq;
using Shoplify.Web.ViewModels.Notification;

namespace Shoplify.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Services.Interfaces;

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

        public async Task<IActionResult> All(string userId)
        {
            var notifications = await notificationService.GetAllUnReadByUserIdAsync(userId);

            var viewModel = new List<NotificationViewModel>();

            foreach (var notification in notifications)
            {
                viewModel.Add(new NotificationViewModel
                {
                    ActionLink = notification.ActionLink,
                    Id = notification.Id,
                    Text = notification.Text
                });
            }

            return View(viewModel);
        }
    }
}
