namespace Shoplify.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Common;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.Areas.Administration.BindingModels.Advertisement;
    using Shoplify.Web.Areas.Administration.ViewModels.Advertisement;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdvertisementController : Controller
    {
        private readonly IAdvertisementService advertisementService;
        private readonly INotificationService notificationService;
        private readonly IUserAdWishlistService userAdWishlistService;
        private readonly UserManager<User> userManager;

        public AdvertisementController(IAdvertisementService advertisementService, INotificationService notificationService, IUserAdWishlistService userAdWishlistService, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.notificationService = notificationService;
            this.userAdWishlistService = userAdWishlistService;
            this.advertisementService = advertisementService;
        }

        public async Task<IActionResult> All(int page = 1)
        {
            if (page <= 0)
            {
                return Redirect("/Panel/Index");
            }

            var adsCount = await advertisementService.GetAllAdsCountAsync();
            var lastPage = adsCount / GlobalConstants.AdsPerAdminPanelPageCount + 1;

            if (adsCount % GlobalConstants.AdsPerAdminPanelPageCount == 0 && adsCount > 0)
            {
                lastPage -= 1;
            }

            if (page > lastPage)
            {
                return Redirect("/Panel/Index");
            }

            var ads = await advertisementService.GetAllAdsAsync(page, GlobalConstants.AdsPerAdminPanelPageCount);

            var viewModel = new AdvertisementListingPageViewModel
            {
                TotalAdsCount = adsCount,
                CurrentPage = page,
                LastPage = lastPage,
                Ads = new List<AdvertisementViewModel>()
            };

            foreach (var ad in ads)
            {
                var adOwner = await userManager.FindByIdAsync(ad.UserId);

                viewModel.Ads.Add(new AdvertisementViewModel
                {
                    Id = ad.Id,
                    ArchivedOn = ad.ArchivedOn.ToLocalTime().ToString(GlobalConstants.DateTimeFormat),
                    BannedOn = ad.BannedOn.ToLocalTime().ToString(GlobalConstants.DateTimeFormat),
                    CreatedOn = ad.CreatedOn.ToLocalTime().ToString(GlobalConstants.DateTimeFormat),
                    IsArchived = ad.IsArchived,
                    IsPromoted = ad.IsPromoted,
                    IsBanned = ad.IsBanned,
                    Name = ad.Name,
                    OwnerId = ad.UserId,
                    OwnerName = adOwner.UserName,
                    PromotedUntil = ad.PromotedUntil.ToLocalTime().ToString(GlobalConstants.DateTimeFormat),
                    Views = ad.Views
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> CreatedThisWeek()
        {
            var adsCountByDays = await advertisementService.GetNewAdsCountByDaysFromThisWeekAsync();

            return Json(adsCountByDays);
        }

        public async Task<IActionResult> CountByCategories()
        {
            var adsCountByCategories = await advertisementService.GetAdsCountByCategoriesAsync();

            return Json(adsCountByCategories);
        }

        public IActionResult Promote(string adId)
        {
            if (!advertisementService.Contains(adId))
            {
                return RedirectToAction("All");
            }

            return View(new AdvertisementPromoteViewModel{ Id = adId });
        }

        [HttpPost]
        public async Task<IActionResult> Promote(PromoteBindingModel input)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("All");
            }

            var days = int.Parse(input.Days);
            var ad = await advertisementService.GetByIdAsync(input.Id);

            await advertisementService.PromoteByIdAsync(ad.Id, days);

            var notificationText = $"Your ad has been promoted for {days} days by admin - '{ad.Name}'";
            var actionLink = $"/Advertisement/Details?id={ad.Id}";

            var notification = await notificationService.CreateNotificationAsync(notificationText, actionLink);
            await notificationService.AssignNotificationToUserAsync(notification.Id, ad.UserId);

            return RedirectToAction("All");
        }

        public async Task<IActionResult> UnPromote(string adId)
        {
            var ad = await advertisementService.GetByIdAsync(adId);

            if (ad == null)
            {
                return RedirectToAction("All");
            }

            await advertisementService.UnpromoteByIdAsync(ad.Id);

            var notificationText = $"Your ad has been unpromoted by admin - '{ad.Name}'";
            var actionLink = $"/Advertisement/Details?id={ad.Id}";

            var notification = await notificationService.CreateNotificationAsync(notificationText, actionLink);
            await notificationService.AssignNotificationToUserAsync(notification.Id, ad.UserId);

            return RedirectToAction("All");
        }

        public async Task<IActionResult> Ban(string adId)
        {
            var ad = await advertisementService.GetByIdAsync(adId);

            if (ad == null)
            {
                return RedirectToAction("All");
            }

            await advertisementService.BanByIdAsync(ad.Id);

            var notificationText = $"Your ad has been banned by admin - '{ad.Name}'";
            var actionLink = $"/User/Profile?id={ad.Id}";

            var notificationToAdOwner = await notificationService.CreateNotificationAsync(notificationText, actionLink);
            await notificationService.AssignNotificationToUserAsync(notificationToAdOwner.Id, ad.UserId);

            notificationText = $"Ad in your wishlist has been banned by admin - '{ad.Name}'";
            actionLink = $"/User/Wishlist";

            var usersIds = await userAdWishlistService.GetAllUserIdsThatHaveAdInWishlistAsync(ad.Id);

            var notificationToAllUsersThatHaveAdInWishlist = await notificationService.CreateNotificationAsync(notificationText, actionLink);
            await notificationService.AssignNotificationToUsersAsync(notificationToAllUsersThatHaveAdInWishlist.Id, usersIds.ToList());

            return RedirectToAction("All");
        }

        public async Task<IActionResult> UnBan(string adId)
        {
            var ad = await advertisementService.GetByIdAsync(adId);

            if (ad == null)
            {
                return RedirectToAction("All");
            }

            await advertisementService.UnbanByIdAsync(ad.Id);

            var notificationText = $"Your ad has been unbanned by admin - '{ad.Name}'";
            var actionLink = $"/User/Profile?id={ad.Id}";

            var notificationToAdOwner = await notificationService.CreateNotificationAsync(notificationText, actionLink);
            await notificationService.AssignNotificationToUserAsync(notificationToAdOwner.Id, ad.UserId);

            notificationText = $"Ad in your wishlist has been unbanned by admin - '{ad.Name}'";
            actionLink = $"/User/Wishlist";

            var usersIds = await userAdWishlistService.GetAllUserIdsThatHaveAdInWishlistAsync(ad.Id);

            var notificationToAllUsersThatHaveAdInWishlist = await notificationService.CreateNotificationAsync(notificationText, actionLink);
            await notificationService.AssignNotificationToUsersAsync(notificationToAllUsersThatHaveAdInWishlist.Id, usersIds.ToList());

            return RedirectToAction("All");
        }

        public async Task<IActionResult> Archive(string adId)
        {
            var ad = await advertisementService.GetByIdAsync(adId);

            if (ad == null)
            {
                return RedirectToAction("All");
            }

            await advertisementService.ArchiveByIdAsync(ad.Id);

            var notificationText = $"Your ad has been archived by admin - '{ad.Name}'";
            var actionLink = $"/User/Profile?id={ad.Id}";

            var notificationToAdOwner = await notificationService.CreateNotificationAsync(notificationText, actionLink);
            await notificationService.AssignNotificationToUserAsync(notificationToAdOwner.Id, ad.UserId);

            notificationText = $"Ad in your wishlist has been archived by admin - '{ad.Name}'";
            actionLink = $"/User/Wishlist";

            var usersIds = await userAdWishlistService.GetAllUserIdsThatHaveAdInWishlistAsync(ad.Id);

            var notificationToAllUsersThatHaveAdInWishlist = await notificationService.CreateNotificationAsync(notificationText, actionLink);
            await notificationService.AssignNotificationToUsersAsync(notificationToAllUsersThatHaveAdInWishlist.Id, usersIds.ToList());

            return RedirectToAction("All");
        }

        public async Task<IActionResult> UnArchive(string adId)
        {
            var ad = await advertisementService.GetByIdAsync(adId);

            if (ad == null)
            {
                return RedirectToAction("All");
            }

            await advertisementService.UnarchiveByIdAsync(ad.Id);

            var notificationText = $"Your ad has been unarchived by admin - '{ad.Name}'";
            var actionLink = $"/User/Profile?id={ad.Id}";

            var notificationToAdOwner = await notificationService.CreateNotificationAsync(notificationText, actionLink);
            await notificationService.AssignNotificationToUserAsync(notificationToAdOwner.Id, ad.UserId);

            notificationText = $"Ad in your wishlist has been unarchived by admin - '{ad.Name}'";
            actionLink = $"/User/Wishlist";

            var usersIds = await userAdWishlistService.GetAllUserIdsThatHaveAdInWishlistAsync(ad.Id);

            var notificationToAllUsersThatHaveAdInWishlist = await notificationService.CreateNotificationAsync(notificationText, actionLink);
            await notificationService.AssignNotificationToUsersAsync(notificationToAllUsersThatHaveAdInWishlist.Id, usersIds.ToList());

            return RedirectToAction("All");
        }
    }
}
