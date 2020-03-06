using System;

namespace Shoplify.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;
    using Domain;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;
    using Shoplify.Services.Interfaces;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private ShoplifyDbContext context;
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;

        private ICategoryService categoryService;
        private ISubCategoryService subCategoryService;
        private IAdvertisementService advertisementService;

        public HomeController(ILogger<HomeController> logger, ShoplifyDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ICategoryService categoryService, ISubCategoryService subCategoryService, IAdvertisementService advertisementService)
        {
            this.logger = logger;
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.categoryService = categoryService;
            this.subCategoryService = subCategoryService;
            this.advertisementService = advertisementService;
        }

        public async Task<IActionResult> Index()
        {
            var date = DateTime.UtcNow;

            await advertisementService.ArchiveAllExpiredAdsAsync(date);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
