using System.Linq;
using Shoplify.Services.Interfaces;
using Shoplify.Services.Mapping;
using Shoplify.Services.Seeding;
using Shoplify.Web.BindingModels.Advertisement;
using Shoplify.Web.ViewModels.Advertisement;
using Shoplify.Web.ViewModels.Category;

namespace Shoplify.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AutoValidateAntiforgeryToken]
    public class AdvertisementController : Controller
    {
        private ICategoryService categoryService;

        public AdvertisementController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var categories = categoryService.GetAll().To<CategoryDropdownViewModel>().ToList();
            var viewModel = new CreateViewModel()
            {
                BindingModel = new CreateAdvertisementBindingModel(),
                Categories = categories,
                Towns = towns,
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvertisementBindingModel advertisement)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/Advertisement/Create");
            }

            return Redirect("/Home/Test");
        }
    }
}
