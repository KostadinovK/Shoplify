using System.Security.Claims;
using Shoplify.Services.Models.Report;
using Shoplify.Web.BindingModels.Report;

namespace Shoplify.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Services.Interfaces;

    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ReportController : Controller
    {
        private readonly IAdvertisementService advertisementService;
        private readonly IReportService reportService;

        public ReportController(IAdvertisementService advertisementService, IReportService reportService)
        {
            this.advertisementService = advertisementService;
            this.reportService = reportService;
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

            return Redirect($"/Advertisement/Details?id={input.ReportedAdvertisementId}");
        }
    }
}
