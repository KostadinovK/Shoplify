using System;
using System.Collections.Generic;
using Shoplify.Common;

namespace Shoplify.Tests.ServicesTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using Shoplify.Domain;
    using Shoplify.Services.Implementations;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.Data;

    [TestFixture]
    public class UserServiceTests
    {
        private ShoplifyDbContext context;
        private IUserService service;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ShoplifyDbContext>()
                .UseInMemoryDatabase(databaseName: "users")
                .Options;

            this.context = new ShoplifyDbContext(options);

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            this.service = new UserService(context);
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.DisposeAsync();
        }

        [Test]
        public async Task IsFollowedByUser_WithNoExistingData_ShouldReturnFalse()
        {
            var expectedResult = await service.IsFollowedByUser("test", "test2");

            Assert.IsFalse(expectedResult);
        }

        [Test]
        public async Task IsFollowedByUser_WithExistingData_ShouldReturnTrue()
        {
            var followingId = "followingId";
            var followerId = "followerId";

            await context.FollowersFollowings.AddAsync(new FollowerFollowing
            {
                FollowingId = "followingId",
                FollowerId = "followerId",
            });

            await context.SaveChangesAsync();

            var expectedResult = await service.IsFollowedByUser(followerId, followingId);

            Assert.IsTrue(expectedResult);
        }

        [Test]
        public async Task FollowUserAsync_WithValidData_ShouldAddCorrectly()
        {
            var followerId = "follower";
            var followingId = "following";

            await service.FollowUserAsync(followerId, followingId);

            var actualCount = context.FollowersFollowings.Count();
            var expectedCount = 1;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task UnfollowUserAsync_WithValidData_ShouldRemoveCorrectly()
        {
            var followerId = "follower";
            var followingId = "following";

            await service.FollowUserAsync(followerId, followingId);

            await service.UnfollowUserAsync(followerId, followingId);

            var actualCount = context.FollowersFollowings.Count();
            var expectedCount = 0;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllUserIdsThatAreFollowingUserAsync_ShouldReturnCorrectly()
        {
            var followerId = "follower";
            var followingId = "following";

            await service.FollowUserAsync(followerId, followingId);

            var userIds = await service.GetAllUserIdsThatAreFollowingUserAsync(followingId);

            var expectedResult = 1;
            var actualResult = userIds.Count();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task BanUserByIdAsync_WithInvalidUserId_ShouldReturnFalse()
        {
            var userId = "invalid";

            var result = await service.BanUserByIdAsync(userId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task BanUserByIdAsync_WithValidUserId_ShouldReturnTrue()
        {
            var user = new User
            {
                UserName = "test",
                NormalizedUserName = "TEST",
                RegisteredOn = DateTime.UtcNow,
                IsBanned = false,
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var result = await service.BanUserByIdAsync(user.Id);

            Assert.IsTrue(result);
            Assert.IsTrue(user.IsBanned);
        }

        [Test]
        public async Task UnbanUserByIdAsync_WithInvalidUserId_ShouldReturnFalse()
        {
            var userId = "invalid";

            var result = await service.UnbanUserByIdAsync(userId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task UnbanUserByIdAsync_WithValidUserId_ShouldReturnTrue()
        {
            var user = new User
            {
                UserName = "test",
                NormalizedUserName = "TEST",
                RegisteredOn = DateTime.UtcNow,
                IsBanned = true,
                BannedOn = DateTime.UtcNow,
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var result = await service.UnbanUserByIdAsync(user.Id);

            Assert.IsTrue(result);
            Assert.IsFalse(user.IsBanned);
        }

        [Test]
        public async Task GetAllUserCountWithoutAdminAsync_ShouldReturnCorrectly()
        {
            var user = new User
            {
                UserName = "test",
                NormalizedUserName = "TEST",
                RegisteredOn = DateTime.UtcNow,
                IsBanned = false,
            };

            var admin = new User
            {
                UserName = GlobalConstants.AdminUserName,
                NormalizedUserName = "ADMIN",
                RegisteredOn = DateTime.UtcNow,
                IsBanned = false,
            };

            await context.Users.AddAsync(user);
            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();

            var actualCount = await service.GetAllUserCountWithoutAdminAsync();
            var expectedCount = 1;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllUsersWithoutAdminAsync_ShouldReturnCorrectly()
        {
            var orderBy = "nameAsc";

            var user = new User
            {
                UserName = "test",
                NormalizedUserName = "TEST",
                RegisteredOn = DateTime.UtcNow,
                IsBanned = false,
            };

            var user2 = new User
            {
                UserName = "asc",
                NormalizedUserName = "ASC",
                RegisteredOn = DateTime.UtcNow,
                IsBanned = false,
            };

            var admin = new User
            {
                UserName = GlobalConstants.AdminUserName,
                NormalizedUserName = "ADMIN",
                RegisteredOn = DateTime.UtcNow,
                IsBanned = false,
            };

            await context.Users.AddAsync(user);
            await context.Users.AddAsync(user2);
            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();

            var users = await service.GetAllUsersWithoutAdminAsync(1, 10, orderBy);
            var actualCount = users.Count();
            var expectedCount = 2;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllUsersWithoutAdminAsync_ShouldReturnCorrectlyPages()
        {
            var page = 1;
            var usersPerPage = 1;
            var orderBy = "nameAsc";

            var user = new User
            {
                UserName = "test",
                NormalizedUserName = "TEST",
                RegisteredOn = DateTime.UtcNow,
                IsBanned = false,
            };

            var user2 = new User
            {
                UserName = "asc",
                NormalizedUserName = "ASC",
                RegisteredOn = DateTime.UtcNow,
                IsBanned = false,
            };

            var admin = new User
            {
                UserName = GlobalConstants.AdminUserName,
                NormalizedUserName = "ADMIN",
                RegisteredOn = DateTime.UtcNow,
                IsBanned = false,
            };

            await context.Users.AddAsync(user);
            await context.Users.AddAsync(user2);
            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();

            var users = await service.GetAllUsersWithoutAdminAsync(page, usersPerPage, orderBy);
            var actualCount = users.Count();
            var expectedCount = usersPerPage;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetNewUsersCountByDaysFromThisWeekAsync_WithNoUsersAtAll_ShouldReturnCorrectly()
        {
            Dictionary<string, int> usersCountByDays = await service.GetNewUsersCountByDaysFromThisWeekAsync();

            var expectedCount = 0;

            foreach (var kvp in usersCountByDays)
            {
                var actualCount = kvp.Value;

                Assert.AreEqual(expectedCount, actualCount);
            }
        }

        [Test]
        public async Task GetNewUsersCountByDaysFromThisWeekAsync_WithNoUsersThisWeek_ShouldReturnCorrectly()
        {
            var user = new User
            {
                UserName = "test",
                NormalizedUserName = "TEST",
                RegisteredOn = DateTime.UtcNow.AddDays(-10),
                IsBanned = false,
            };

            var user2 = new User
            {
                UserName = "test",
                NormalizedUserName = "TEST",
                RegisteredOn = DateTime.UtcNow.AddDays(-7),
                IsBanned = false,
            };

            await context.Users.AddAsync(user);
            await context.Users.AddAsync(user2);
            await context.SaveChangesAsync();

            Dictionary<string, int> usersCountByDays = await service.GetNewUsersCountByDaysFromThisWeekAsync();

            var expectedCount = 0;

            foreach (var kvp in usersCountByDays)
            {
                var actualCount = kvp.Value;

                Assert.AreEqual(expectedCount, actualCount);
            }
        }

        [Test]
        public async Task GetNewUsersCountByDaysFromThisWeekAsync_WithUsersThisWeek_ShouldReturnCorrectly()
        {
            var todayDate = DateTime.UtcNow;
            var yesterdayDate = todayDate.AddDays(-1);

            var oldUser = new User
            {
                UserName = "test",
                NormalizedUserName = "TEST",
                RegisteredOn = todayDate.AddDays(-10),
                IsBanned = false,
            };

            var newUser = new User
            {
                UserName = "test",
                NormalizedUserName = "TEST",
                RegisteredOn = todayDate,
                IsBanned = false,
            };

            var newUser2 = new User
            {
                UserName = "test",
                NormalizedUserName = "TEST",
                RegisteredOn = yesterdayDate,
                IsBanned = false,
            };

            var newUser3 = new User
            {
                UserName = "test",
                NormalizedUserName = "TEST",
                RegisteredOn = yesterdayDate,
                IsBanned = false,
            };

            await context.Users.AddAsync(oldUser);
            await context.Users.AddAsync(newUser);
            await context.Users.AddAsync(newUser2);
            await context.Users.AddAsync(newUser3);
            await context.SaveChangesAsync();

            Dictionary<string, int> usersCountByDays = await service.GetNewUsersCountByDaysFromThisWeekAsync();

            var expectedCount = 0;

            foreach (var kvp in usersCountByDays)
            {
                if (kvp.Key == todayDate.DayOfWeek.ToString())
                {
                    expectedCount = 1;
                }else if (kvp.Key == yesterdayDate.DayOfWeek.ToString())
                {
                    expectedCount = 2;
                }
                else
                {
                    expectedCount = 0;
                }

                var actualCount = kvp.Value;

                Assert.AreEqual(expectedCount, actualCount);
            }
        }
    }
}
