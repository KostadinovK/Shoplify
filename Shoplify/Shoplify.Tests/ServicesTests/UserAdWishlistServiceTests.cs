namespace Shoplify.Tests.ServicesTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using NUnit.Framework;
    using Shoplify.Domain;
    using Shoplify.Services.Implementations;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.Data;

    [TestFixture]
    public class UserAdWishlistServiceTests
    {
        private ShoplifyDbContext context;
        private IUserAdWishlistService service;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ShoplifyDbContext>()
                .UseInMemoryDatabase(databaseName: "wishlists")
                .Options;

            this.context = new ShoplifyDbContext(options);

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            this.service = new UserAdWishlistService(context);
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.DisposeAsync();
        }

        [Test]
        public async Task IsAdInWishlistAsync_WithNoExistingData_ShouldReturnFalse()
        {
            var expectedResult = await service.IsAdInWishlistAsync("test", "test");

            Assert.IsFalse(expectedResult);
        }

        [Test]
        public async Task IsAdInWishlistAsync_WithExistingData_ShouldReturnTrue()
        {
            await context.UsersAdvertisementsWishlist.AddAsync(new UserAdvertisementWishlist
            {
                UserId = "userId",
                AdvertisementId = "adId"
            });

            await context.SaveChangesAsync();

            var expectedResult = await service.IsAdInWishlistAsync("userId", "adId");

            Assert.IsTrue(expectedResult);
        }

        [Test]
        public async Task AddToWishlistAsync_WithValidData_ShouldAddCorrectly()
        {
            var userId = "userId";
            var adId = "adId";

            await service.AddToWishlistAsync(userId, adId);

            var actualCount = context.UsersAdvertisementsWishlist.Count();
            var expectedCount = 1;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task RemoveFromWishlistAsync_WithValidData_ShouldRemoveCorrectly()
        {
            var userId = "userId";
            var adId = "adId";

            await service.AddToWishlistAsync(userId, adId);

            await service.RemoveFromWishlistAsync(userId, adId);

            var actualCount = context.UsersAdvertisementsWishlist.Count();
            var expectedCount = 0;

            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
