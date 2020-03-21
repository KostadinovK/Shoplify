using System.Collections.Generic;
using Shoplify.Web.Areas.Administration.ViewModels.Advertisement;
using Shoplify.Web.Areas.Administration.ViewModels.User;

namespace Shoplify.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Common;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdvertisementController : Controller
    {
        private readonly IAdvertisementService advertisementService;
        private readonly UserManager<User> userManager;

        public AdvertisementController(IAdvertisementService advertisementService, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.advertisementService = advertisementService;
        }

        public async Task<IActionResult> All(int page)
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
    }
}
