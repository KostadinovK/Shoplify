namespace Shoplify.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
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
        private readonly UserManager<User> userManager;

        public AdvertisementController(IAdvertisementService advertisementService, INotificationService notificationService, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.notificationService = notificationService;
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
                    ArchivedOn = ad.ArchivedOn,
                    BannedOn = ad.BannedOn,
                    CreatedOn = ad.CreatedOn,
                    IsArchived = ad.IsArchived,
                    IsPromoted = ad.IsPromoted,
                    IsBanned = ad.IsBanned,
                    Name = ad.Name,
                    OwnerId = ad.UserId,
                    OwnerName = adOwner.UserName,
                    PromotedUntil = ad.PromotedUntil,
                    Views = ad.Views
                });
            }

            return View(viewModel);
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

            var notificationText = $"Your ad has been promoted for {days} days by admin - {ad.Name}";
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

            var notificationText = $"Your ad has been unpromoted by admin - {ad.Name}";
            var actionLink = $"/Advertisement/Details?id={ad.Id}";

            var notification = await notificationService.CreateNotificationAsync(notificationText, actionLink);
            await notificationService.AssignNotificationToUserAsync(notification.Id, ad.UserId);

            return RedirectToAction("All");
        }
    }
}
