namespace Shoplify.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Data;
    using Domain;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;
    using Services;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private ITestService test;
        private ShoplifyDbContext context;
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;

        public HomeController(ILogger<HomeController> logger, ITestService test, ShoplifyDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.logger = logger;
            this.test = test;
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Test()
        {
            //test.TestAutoMapper();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var adminRole = new IdentityRole("Admin");
            var userRole = new IdentityRole("User");

            await roleManager.CreateAsync(adminRole);
            await roleManager.CreateAsync(userRole);

            var user = new User()
            {
                UserName = "test",
                Email = "test@abv.bg",
                RegisteredOn = DateTime.UtcNow,
                IsBanned = false
            };

            var result = await userManager.CreateAsync(user, "test");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }

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
