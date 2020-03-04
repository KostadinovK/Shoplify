namespace Shoplify.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task FollowUserAsync(string userId, string userToFollowId);

        Task UnfollowUserAsync(string userId, string userToUnfollowId);

        Task<bool> IsFollowedByUser(string firstUserId, string secondUserId);
    }
}
