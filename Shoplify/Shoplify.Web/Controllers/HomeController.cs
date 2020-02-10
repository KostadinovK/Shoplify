using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
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
        private ISubCategoryService subCategoryService;

        public HomeController(ILogger<HomeController> logger, ShoplifyDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ICategoryService categoryService, ISubCategoryService subCategoryService)
        {
            this.logger = logger;
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.categoryService = categoryService;
            this.subCategoryService = subCategoryService;
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
            var category = await categoryService.GetByNameAsync("Home");

            var subCategory = new SubCategoryServiceModel
            {
                Name = "test",
                CategoryId = category.Id
            };

            await subCategoryService.CreateAsync(subCategory);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
