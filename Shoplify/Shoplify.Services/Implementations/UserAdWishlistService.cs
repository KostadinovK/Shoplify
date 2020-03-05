using System.Collections.Generic;
using Shoplify.Common;
using Shoplify.Services.Models;

namespace Shoplify.Services.Implementations
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.Data;

    public class UserAdWishlistService : IUserAdWishlistService
    {
        private readonly ShoplifyDbContext context;

        public UserAdWishlistService(ShoplifyDbContext context)
        {
            this.context = context;
        }

        public async Task AddToWishlistAsync(string userId, string adId)
        {
            if (!await IsAdInWishlistAsync(userId, adId))
            {
                await context.UsersAdvertisementsWishlist.AddAsync(new UserAdvertisementWishlist
                {
                    UserId = userId,
                    AdvertisementId = adId
                });

                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromWishlistAsync(string userId, string adId)
        {
            if (await IsAdInWishlistAsync(userId, adId))
            {
                var model = context.UsersAdvertisementsWishlist.SingleOrDefault(ua =>
                    ua.AdvertisementId == adId && ua.UserId == userId);

                context.UsersAdvertisementsWishlist.Remove(model);

                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsAdInWishlistAsync(string userId, string adId)
        {
            return await context.UsersAdvertisementsWishlist.AnyAsync(ua =>
                ua.AdvertisementId == adId && ua.UserId == userId);
        }

        public async Task<int> GetWishlistCountAsync(string userId)
        {
            return await context.UsersAdvertisementsWishlist.CountAsync(ua =>
                ua.UserId == userId && ua.Advertisement.IsArchived == false && ua.Advertisement.IsBanned == false);
        }

        public async Task<IEnumerable<AdvertisementViewServiceModel>> GetUserWishlistAsync(string userId, int page, int adsPerPage)
        {
            var ads22 = context.UsersAdvertisementsWishlist.ToList();
            var ads = await context.UsersAdvertisementsWishlist
                .Where(ua =>
                ua.UserId == userId && ua.Advertisement.IsArchived == false && ua.Advertisement.IsBanned == false)
                .Select(ua => ua.Advertisement)
                .Take(adsPerPage * page)
                .Skip((page - 1) * adsPerPage)
                .ToListAsync();

            var result = ads.Select(a => new AdvertisementViewServiceModel
            {
                CategoryId = a.CategoryId,
                CreatedOn = a.CreatedOn.ToLocalTime(),
                Id = a.Id,
                SubCategoryId = a.SubCategoryId,
                Name = a.Name,
                TownId = a.TownId,
                UserId = a.UserId,
                Price = a.Price
            });

            return result;
        }
    }
}
