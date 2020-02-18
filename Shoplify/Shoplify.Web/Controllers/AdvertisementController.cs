using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Shoplify.Domain;
using Shoplify.Services.Models;
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
        private IAdvertisementService advertisementService;
        private readonly UserManager<User> userManager;

        public AdvertisementController(IAdvertisementService advertisementService, UserManager<User> userManager)
        {
            this.advertisementService = advertisementService;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult Create()
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var advertisementServiceModel = new AdvertisementCreateServiceModel
            {
                Name = advertisement.Name,
                Price = advertisement.Price,
                Description = advertisement.Description,
                Condition = advertisement.Condition,
                CategoryId = advertisement.CategoryId,
                SubCategoryId = advertisement.SubCategoryId,
                TownId = advertisement.TownId,
                Address = advertisement.Address,
                Number = advertisement.Number,
                UserId = userId,
                Images = advertisement.Images
            };

            await advertisementService.CreateAsync(advertisementServiceModel);

            return Redirect("/Home/Index");
        }

        [Authorize]
        public async Task<IActionResult> GetByCategory(string categoryId)
        {
            var ads = await advertisementService.GetByCategoryIdAsync(categoryId);

            return Json(ads);
        }
    }
}
