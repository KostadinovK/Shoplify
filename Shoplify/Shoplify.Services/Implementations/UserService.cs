namespace Shoplify.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.Data;

    public class UserService : IUserService
    {
        private readonly ShoplifyDbContext context;

        public UserService(ShoplifyDbContext context)
        {
            this.context = context;
        }

        public async Task FollowUserAsync(string userId, string userToFollowId)
        {
            if (!await IsFollowedByUser(userId, userToFollowId))
            {
                await context.FollowersFollowings.AddAsync(new FollowerFollowing()
                {
                    FollowerId = userId,
                    FollowingId = userToFollowId
                });

                await context.SaveChangesAsync();
            }
        }

        public async Task UnfollowUserAsync(string userId, string userToUnfollowId)
        {
            if (await IsFollowedByUser(userId, userToUnfollowId))
            {
                var model = context.FollowersFollowings.SingleOrDefault(ff =>
                    ff.FollowerId == userId && ff.FollowingId == userToUnfollowId);

                context.FollowersFollowings.Remove(model);

                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsFollowedByUser(string firstUserId, string secondUserId)
        {
            return await context.FollowersFollowings.AnyAsync(ff =>
                ff.FollowerId == firstUserId && ff.FollowingId == secondUserId);
        }

        public async Task<IEnumerable<string>> GetAllUserIdsThatAreFollowingUserAsync(string userId)
        {
            return await context.FollowersFollowings
                .Where(ff => ff.FollowingId == userId)
                .Select(ff => ff.FollowerId)
                .ToListAsync();
        }
    }
}
