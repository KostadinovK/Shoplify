namespace Shoplify.Services.Implementations
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

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
            if (!context.UsersAdvertisementsWishlist.Any(ua => ua.AdvertisementId == adId && ua.UserId == userId))
            {
                await context.UsersAdvertisementsWishlist.AddAsync(new UserAdvertisementWishlist
                {
                    UserId = userId,
                    AdvertisementId = adId
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
