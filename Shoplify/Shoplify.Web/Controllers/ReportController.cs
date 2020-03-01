using System.Security.Claims;
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

        public ReportController(IAdvertisementService advertisementService)
        {
            this.advertisementService = advertisementService;
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
            return View();
        }
    }
}
