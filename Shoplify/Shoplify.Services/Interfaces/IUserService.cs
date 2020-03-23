namespace Shoplify.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Services.Models.User;

    public interface IUserService
    {
        Task FollowUserAsync(string userId, string userToFollowId);

        Task UnfollowUserAsync(string userId, string userToUnfollowId);

        Task<bool> IsFollowedByUser(string firstUserId, string secondUserId);

        Task<IEnumerable<string>> GetAllUserIdsThatAreFollowingUserAsync(string userId);

        Task<bool> BanUserByIdAsync(string id);

        Task<bool> UnbanUserByIdAsync(string id);

        Task<int> GetAllUserCountWithoutAdminAsync();

        Task<IEnumerable<UserServiceModel>> GetAllUsersWithoutAdminAsync(int page, int usersPerPage, string orderBy);

        Task<string> GetAdminIdAsync();
    }
}
