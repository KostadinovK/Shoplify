using Shoplify.Domain.Enums;
using Shoplify.Services.Models;

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
        private IAdvertisementService adService;
        private IFormFile mockedFile;

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

            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqIFormFile = new Mock<IFormFile>();

            this.mockedFile = moqIFormFile.Object;

            moqCloudinaryService.Setup(x => x.UploadPictureAsync(moqIFormFile.Object, "FileName"))
                .ReturnsAsync("http://test.com");

            this.adService = new AdvertisementService(context, moqCloudinaryService.Object);
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

        [Test]
        public async Task GetUserWishlistAsync_WithInvalidUserId_ShouldReturnEmptyCollection()
        {
            var advertisement = new UserAdvertisementWishlist
            {
               UserId = "test",
               AdvertisementId = "test"
            };

            await context.UsersAdvertisementsWishlist.AddAsync(advertisement);

            var ads = await service.GetUserWishlistAsync("empty", 1, 1);

            var actualResult = ads.Count();
            var expectedResult = 0;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task GetUserWishlistAsync_WithValidUserId_ShouldReturnCorrectly()
        {
            var advertisement = new AdvertisementCreateServiceModel()
            {
                Name = "OnePlus 7 Pro",
                Description = "cool phone for everyday use, excellent performance",
                Price = 800,
                Condition = ProductCondition.New,
                CategoryId = "Electronics",
                SubCategoryId = "Phone",
                TownId = "testTownId",
                Address = "str nqkoq",
                Number = "telefonce",
                UserId = "test1"
            };

            await adService.CreateAsync(advertisement);

            var ad = await context.Advertisements.FirstOrDefaultAsync(a => a.Name == "OnePlus 7 Pro");

            var advertisementWishlist = new UserAdvertisementWishlist
            {
                UserId = "test",
                AdvertisementId = ad.Id,
                Advertisement = ad,
            };

            await context.UsersAdvertisementsWishlist.AddAsync(advertisementWishlist);
            await context.SaveChangesAsync();

            var ads = await service.GetUserWishlistAsync("test", 1, 1);

            var actualResult = ads.Count();
            var expectedResult = 1;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task GetWishlistCountAsync_WithNoAds_ShouldReturnCorrectly()
        {
            var expectedResult = 0;

            var actualResult = await service.GetWishlistCountAsync("test");

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task GetWishlistCountAsync_WithAds_ShouldReturnCorrectly()
        {
            var advertisement = new AdvertisementCreateServiceModel()
            {
                Name = "OnePlus 7 Pro",
                Description = "cool phone for everyday use, excellent performance",
                Price = 800,
                Condition = ProductCondition.New,
                CategoryId = "Electronics",
                SubCategoryId = "Phone",
                TownId = "testTownId",
                Address = "str nqkoq",
                Number = "telefonce",
                UserId = "test"
            };

            await adService.CreateAsync(advertisement);

            var ad = await context.Advertisements.FirstOrDefaultAsync(a => a.Name == "OnePlus 7 Pro");

            var advertisementWishlist = new UserAdvertisementWishlist
            {
                UserId = "test",
                AdvertisementId = ad.Id,
                Advertisement = ad,
            };

            await context.UsersAdvertisementsWishlist.AddAsync(advertisementWishlist);
            await context.SaveChangesAsync();

            var expectedResult = 1;

            var actualResult = await service.GetWishlistCountAsync("test");

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
