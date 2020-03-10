namespace Shoplify.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Shoplify.Services.Interfaces;

    public class HomeController : Controller
    {
        private IAdvertisementService advertisementService;

        public HomeController(IAdvertisementService advertisementService)
        {
            this.advertisementService = advertisementService;
        }

        public async Task<IActionResult> Index()
        {
            var date = DateTime.UtcNow;

            await advertisementService.ArchiveAllExpiredAdsAsync(date);
            await advertisementService.UnPromoteAllExpiredAdsAsync(date);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
