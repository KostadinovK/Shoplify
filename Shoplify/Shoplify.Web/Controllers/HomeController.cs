using System.Collections.Generic;
using Shoplify.Services.Interfaces;
using Shoplify.Services.Models;

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
        private ShoplifyDbContext context;
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;

        private ICategoryService categoryService;

        public HomeController(ILogger<HomeController> logger, ShoplifyDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ICategoryService categoryService)
        {
            this.logger = logger;
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Test()
        {
            var names = new List<string>() {"home", "test"};

            await categoryService.CreateAllAsync(names);

            var names2 = new List<string>() { "tedf", "sfsf" };
            var icons = new List<string>() { "tedf", "sfsf" };

            await categoryService.CreateAllAsync(names2, icons);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
