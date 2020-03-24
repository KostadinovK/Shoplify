using Microsoft.JSInterop.Infrastructure;

namespace Shoplify.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Shoplify.Common;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models.User;
    using Shoplify.Web.Data;

    public class UserService : IUserService
    {
        private const string NoAdminErrorMessage = "No Admin found";

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

        public async Task<bool> BanUserByIdAsync(string id)
        {
            var user = context.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                return false;
            }

            user.IsBanned = true;
            user.BannedOn = DateTime.UtcNow;

            context.Users.Update(user);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnbanUserByIdAsync(string id)
        {
            var user = context.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                return false;
            }

            user.IsBanned = false;
            user.BannedOn = null;

            context.Users.Update(user);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetAllUserCountWithoutAdminAsync()
        {
            return await context.Users.CountAsync(u => u.UserName != GlobalConstants.AdminUserName);
        }

        public async Task<IEnumerable<UserServiceModel>> GetAllUsersWithoutAdminAsync(int page, int usersPerPage, string orderBy)
        {
            var users = context.Users
                .Where(u => u.UserName != GlobalConstants.AdminUserName);

            var orderedUsers = new List<User>();

            if (orderBy == "nameDesc")
            {
                orderedUsers = await users
                    .OrderByDescending(u => u.UserName)
                    .Take(page * usersPerPage)
                    .Skip((page - 1) * usersPerPage)
                    .ToListAsync();

            }
            else if (orderBy == "nameAsc")
            {
                orderedUsers = await users
                    .OrderBy(a => a.UserName)
                    .Take(page * usersPerPage)
                    .Skip((page - 1) * usersPerPage)
                    .ToListAsync();
            }
            else if (orderBy == "dateAsc")
            {
                orderedUsers = await users
                    .OrderBy(a => a.RegisteredOn)
                    .Take(page * usersPerPage)
                    .Skip((page - 1) * usersPerPage)
                    .ToListAsync();
            }
            else if (orderBy == "dateDesc")
            {
                orderedUsers = await users
                    .OrderByDescending(a => a.RegisteredOn)
                    .Take(page * usersPerPage)
                    .Skip((page - 1) * usersPerPage)
                    .ToListAsync();
            }
            else if (orderBy == "bannedAsc")
            {
                orderedUsers = await users
                    .OrderBy(a => a.IsBanned)
                    .Take(page * usersPerPage)
                    .Skip((page - 1) * usersPerPage)
                    .ToListAsync();
            }
            else if (orderBy == "bannedDesc")
            {
                orderedUsers = await users
                    .OrderByDescending(a => a.IsBanned)
                    .Take(page * usersPerPage)
                    .Skip((page - 1) * usersPerPage)
                    .ToListAsync();
            }
            else
            {
                orderedUsers = await users
                    .OrderByDescending(a => a.RegisteredOn)
                    .Take(page * usersPerPage)
                    .Skip((page - 1) * usersPerPage)
                    .ToListAsync();
            }

            var result = orderedUsers.Select(u => new UserServiceModel
            {
                Id = u.Id,
                IsBanned = u.IsBanned,
                BannedOn = u.BannedOn.GetValueOrDefault().ToLocalTime(),
                Username = u.UserName,
                RegisteredOn = u.RegisteredOn
            })
                .ToList();

            return result;
        }

        public async Task<string> GetAdminIdAsync()
        {
            var admin = await context.Users
                .SingleOrDefaultAsync(u => u.UserName == GlobalConstants.AdminUserName);

            if (admin == null)
            {
                throw new InvalidOperationException(NoAdminErrorMessage);
            }

            return admin.Id;
        }

        public async Task<Dictionary<string, int>> GetNewUsersCountByDaysFromThisWeekAsync()
        {
            var todayDate = DateTime.UtcNow;
            var startOfWeek = todayDate.AddDays(-6);

            var result = new Dictionary<string, int>();

            for (int i = 0; i < 7; i++)
            {
                result.Add(startOfWeek.AddDays(i).DayOfWeek.ToString(), 0);
            }

            var daysOfWeek = await context.Users.Where(u => u.RegisteredOn >= startOfWeek && u.RegisteredOn <= todayDate)
                .Select(u => u.RegisteredOn.DayOfWeek.ToString())
                .ToListAsync();

            foreach (var day in daysOfWeek)
            {
                result[day]++;
            }

            return result;
        }
    }
}
