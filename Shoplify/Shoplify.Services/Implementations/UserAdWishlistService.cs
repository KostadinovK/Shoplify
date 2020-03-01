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
    }
}
