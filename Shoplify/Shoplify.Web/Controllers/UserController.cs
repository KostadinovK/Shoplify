using System.Security.Claims;

namespace Shoplify.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Services.Interfaces;

    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class UserController : Controller
    {
        private readonly IAdvertisementService advertisementService;
        private readonly IUserAdWishlistService userAdWishlistService;

        public UserController(IAdvertisementService advertisementService, IUserAdWishlistService userAdWishlistService)
        {
            this.advertisementService = advertisementService;
            this.userAdWishlistService = userAdWishlistService;
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

    }
}
