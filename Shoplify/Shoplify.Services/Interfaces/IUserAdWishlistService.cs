namespace Shoplify.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Services.Models;

    public interface IUserAdWishlistService
    {
        Task AddToWishlistAsync(string userId, string adId);

        Task RemoveFromWishlistAsync(string userId, string adId);

        Task<bool> IsAdInWishlistAsync(string userId, string adId);

        Task<int> GetWishlistCountAsync(string userId);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetUserWishlistAsync(string userId, int page, int adsPerPage);
    }
}
