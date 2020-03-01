namespace Shoplify.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IUserAdWishlistService
    {
        Task AddToWishlistAsync(string userId, string adId);

        Task RemoveFromWishlistAsync(string userId, string adId);

        Task<bool> IsAdInWishlistAsync(string userId, string adId);
    }
}
