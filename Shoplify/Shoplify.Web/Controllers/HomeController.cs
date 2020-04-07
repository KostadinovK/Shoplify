namespace Shoplify.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Shoplify.Services.Interfaces;

    public class HomeController : Controller
    {
        private readonly IAdvertisementService advertisementService;

        public HomeController(IAdvertisementService advertisementService, TelemetryClient telemetryClient)
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

        public IActionResult PageNotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
