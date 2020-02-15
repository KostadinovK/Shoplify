namespace Shoplify.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Mapping;
    using Shoplify.Web.ViewModels.Advertisement;
    using Shoplify.Web.ViewModels.Category;
    using Shoplify.Web.ViewModels.Town;

    [AutoValidateAntiforgeryToken]
    public class AdvertisementController : Controller
    {
        private ICategoryService categoryService;
        private ITownService townService;

        public AdvertisementController(ICategoryService categoryService, ITownService townService)
        {
            this.categoryService = categoryService;
            this.townService = townService;
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var categories = categoryService.GetAll().To<CategoryDropdownViewModel>().ToList();
            var towns = townService.GetAll().To<TownDropdownViewModel>().ToList();

            var viewModel = new CreateViewModel()
            {
                Categories = categories,
                Towns = towns,
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel advertisement)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/Advertisement/Create");
            }

            return Redirect("/Home/Test");
        }
    }
}
