namespace Shoplify.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Common;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.Areas.Administration.ViewModels.Panel;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class PanelController : Controller
    {
        private readonly IUserService userService;
        private readonly IAdvertisementService advertisementService;
        private readonly IReportService reportService;

        public PanelController(IUserService userService, IAdvertisementService advertisementService, IReportService reportService)
        {
            this.userService = userService;
            this.advertisementService = advertisementService;
            this.reportService = reportService;
        }

        public async Task<IActionResult> Index()
        {
            var usersCount = await userService.GetAllUserCountWithoutAdminAsync();
            var adsCount = await advertisementService.GetAllAdsCountAsync();
            var reportsCount = await reportService.GetAllUnArchivedCountAsync();

            var viewModel = new PanelViewModel
            {
                UsersCount = usersCount,
                AdsCount = adsCount,
                ReportsCount = reportsCount
            };

            return View(viewModel);
        }
    }
}
