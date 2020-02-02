using Shoplify.Services;

namespace Shoplify.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private ITestService test;

        public HomeController(ILogger<HomeController> logger, ITestService test)
        {
            this.logger = logger;
            this.test = test;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test()
        {
            test.TestAutoMapper();
            return View();
        }

        public IActionResult Privacy()
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
