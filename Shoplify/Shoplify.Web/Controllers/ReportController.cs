using Microsoft.AspNetCore.Identity;
using Shoplify.Domain;

namespace Shoplify.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models.Report;
    using Shoplify.Web.BindingModels.Report;

    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ReportController : Controller
    {
        private readonly IAdvertisementService advertisementService;
        private readonly IReportService reportService;
        private readonly INotificationService notificationService;
        private readonly UserManager<User> userManager;
        private readonly IUserService userService;

        public ReportController(IAdvertisementService advertisementService, IReportService reportService, INotificationService notificationService, UserManager<User> userManager, IUserService userService)
        {
            this.advertisementService = advertisementService;
            this.reportService = reportService;
            this.notificationService = notificationService;
            this.userManager = userManager;
            this.userService = userService;
        }

        public async Task<IActionResult> Create(string adId)
        {
            var ad = await advertisementService.GetByIdAsync(adId);

            var model = new CreateBindingModel
            {
                ReportedUserId = ad.UserId,
                ReportedAdvertisementId = ad.Id,
                ReportingUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBindingModel input)
        {
            if (!ModelState.IsValid)
            {
                return Redirect($"/Report/Create?adId={input.ReportedAdvertisementId}");
            }

            var serviceModel = new ReportCreateServiceModel
            {
                Description = input.Description,
                ReportedAdvertisementId = input.ReportedAdvertisementId,
                ReportedUserId = input.ReportedUserId,
                ReportingUserId = input.ReportingUserId
            };

            await reportService.CreateAsync(serviceModel);

            var reportingUser = await userManager.FindByIdAsync(input.ReportingUserId);
            var reportedAd = await advertisementService.GetByIdAsync(input.ReportedAdvertisementId);

            var notificationText = $"{reportingUser.UserName} reported one of your ads - {reportedAd.Name}. '{input.Description}'";
            var actionLink = $"Advertisement/Details?id={reportedAd.Id}";

            var notification = await notificationService.CreateNotificationAsync(notificationText, actionLink);
            await notificationService.AssignNotificationToUserAsync(notification.Id, input.ReportedUserId);

            notificationText = $"{reportingUser.UserName} reported an ad - {reportedAd.Name} because of '{input.Description}'";
            actionLink = $"/Administration/Report/All";

            var notificationToAdmin = await notificationService.CreateNotificationAsync(notificationText, actionLink);
            await notificationService.AssignNotificationToUserAsync(notificationToAdmin.Id, await userService.GetAdminIdAsync());

            return Redirect($"/Advertisement/Details?id={input.ReportedAdvertisementId}");
        }
    }
}
