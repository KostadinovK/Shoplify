using System.Linq;
using Shoplify.Domain;

namespace Shoplify.Tests.ServicesTests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
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
    }
}
