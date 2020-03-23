namespace Shoplify.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Common;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.Areas.Administration.ViewModels.Report;

    [Area("Administration")]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class ReportController : Controller
    {
        private UserManager<User> userManager;
        private readonly IReportService reportService;
        private readonly IAdvertisementService advertisementService;
        private readonly INotificationService notificationService;
        private readonly IUserAdWishlistService userAdWishlistService;

        public ReportController(UserManager<User> userManager, IReportService reportService, IAdvertisementService advertisementService, INotificationService notificationService, IUserAdWishlistService userAdWishlistService)
        {
            this.userManager = userManager;
            this.reportService = reportService;
            this.advertisementService = advertisementService;
            this.notificationService = notificationService;
            this.userAdWishlistService = userAdWishlistService;
        }

        public async Task<IActionResult> All(int page = 1)
        {
            if (page <= 0)
            {
                return Redirect("/Panel/Index");
            }

            var reportsCount = await reportService.GetAllUnArchivedCountAsync();
            var lastPage = reportsCount / GlobalConstants.ReportsPerAdminPanelPageCount + 1;

            if (reportsCount % GlobalConstants.ReportsPerAdminPanelPageCount == 0 && reportsCount > 0)
            {
                lastPage -= 1;
            }

            if (page > lastPage)
            {
                return Redirect("/Panel/Index");
            }

            var reports = await reportService.GetAllUnArchivedAsync(page, GlobalConstants.ReportsPerAdminPanelPageCount);

            var viewModel = new ReportListingViewModel
            {
                TotalReportsCount = reportsCount,
                CurrentPage = page,
                LastPage = lastPage,
                Reports = new List<ReportViewModel>()
            };

            foreach (var report in reports)
            {
                var ad = await advertisementService.GetByIdAsync(report.ReportedAdvertisementId);
                var reportingUser = await userManager.FindByIdAsync(report.ReportingUserId);
                var reportedUser = await userManager.FindByIdAsync(report.ReportedUserId);

                viewModel.Reports.Add(new ReportViewModel
                {
                    Id = report.Id,
                    CreatedOn = report.CreatedOn.ToString(GlobalConstants.DateTimeFormat),
                    Description = report.Description,
                    AdvertisementId = ad.Id,
                    AdvertisementName = ad.Name,
                    ReportingUserId = reportingUser.Id,
                    ReportingUserName = reportingUser.UserName,
                    ReportedUserId = reportedUser.Id,
                    ReportedUserName = reportedUser.UserName
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Approve(string reportId)
        {
            var report = await reportService.GetByIdAsync(reportId);
            var ad = await advertisementService.GetByIdAsync(report.ReportedAdvertisementId);
            var adOwner = await userManager.FindByIdAsync(report.ReportedUserId);
            var reportOwner = await userManager.FindByIdAsync(report.ReportingUserId);

            var success = await reportService.ApproveByIdAsync(report.Id);

            if (success)
            {
                var notificationText = $"Your report has been approved by admin for ad - '{ad.Name}'";
                var actionLink = $"/Advertisement/Details?id={ad.Id}";

                var notificationToReportOwner = await notificationService.CreateNotificationAsync(notificationText, actionLink);
                await notificationService.AssignNotificationToUserAsync(notificationToReportOwner.Id, reportOwner.Id);

                notificationText = $"Ad in your wishlist has been banned by admin - '{ad.Name}'";
                actionLink = $"/User/Wishlist";

                var usersIds = await userAdWishlistService.GetAllUserIdsThatHaveAdInWishlistAsync(ad.Id);

                var notificationToAllUsersThatHaveAdInWishlist = await notificationService.CreateNotificationAsync(notificationText, actionLink);
                await notificationService.AssignNotificationToUsersAsync(notificationToAllUsersThatHaveAdInWishlist.Id, usersIds.ToList());

                notificationText = $"Your Ad has been reported and approved by admin - '{ad.Name}' is banned because of {report.Description}";
                actionLink = $"/User/BannedAds";

                var notificationToAdOwner = await notificationService.CreateNotificationAsync(notificationText, actionLink);
                await notificationService.AssignNotificationToUserAsync(notificationToAdOwner.Id, adOwner.Id);
            }

            return RedirectToAction("All");
        }
    }
}
