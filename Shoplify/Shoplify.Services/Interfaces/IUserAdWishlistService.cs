namespace Shoplify.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IUserAdWishlistService
    {
        Task AddToWishlistAsync(string userId, string adId);
    }
}
