using Shoplify.Web.BindingModels.Advertisement;

namespace Shoplify.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Services.Interfaces;

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
            return View(new CreateBindingModel());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateBindingModel advertisement)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/Advertisement/Create");
            }

            return Redirect("/Home/Test");
        }
    }
}
