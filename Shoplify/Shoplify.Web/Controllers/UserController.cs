namespace Shoplify.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;

    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class UserController : Controller
    {
        private readonly IAdvertisementService advertisementService;
        private readonly IUserAdWishlistService userAdWishlistService;
        private readonly UserManager<User> userManager;

        public UserController(IAdvertisementService advertisementService, IUserAdWishlistService userAdWishlistService, UserManager<User> userManager)
        {
            this.advertisementService = advertisementService;
            this.userAdWishlistService = userAdWishlistService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> AddToWishlist(string adId)
        {
            if (!advertisementService.Contains(adId))
            {
                return Redirect("/Home/Index");
            }

            var ad = await advertisementService.GetByIdAsync(adId);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == ad.UserId)
            {
                return Redirect($"/Advertisement/Details?id={adId}");
            }

            await userAdWishlistService.AddToWishlistAsync(userId, adId);

            return Redirect($"/Advertisement/Details?id={adId}");
        }

        public async Task<IActionResult> RemoveFromWishlist(string adId)
        {
            if (!advertisementService.Contains(adId))
            {
                return Redirect("/Home/Index");
            }

            var ad = await advertisementService.GetByIdAsync(adId);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == ad.UserId)
            {
                return Redirect($"/Advertisement/Details?id={adId}");
            }

            await userAdWishlistService.RemoveFromWishlistAsync(userId, adId);

            return Redirect($"/Advertisement/Details?id={adId}");
        }

        public async Task<IActionResult> Profile(string id)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return Redirect("/Home/Index");
            }

            if (user.Id == loggedInUserId)
            {
                return View("LoggedInUserProfile");
            }

            return View();
        }
    }
}
