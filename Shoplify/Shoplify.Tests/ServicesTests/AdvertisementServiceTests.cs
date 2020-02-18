using System.Collections.Generic;
using Shoplify.Domain.Enums;

namespace Shoplify.Tests.ServicesTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using NUnit.Framework;
    using Shoplify.Services.Implementations;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models;
    using Shoplify.Web.Data;

    [TestFixture]
    public class AdvertisementServiceTests
    {
        private ShoplifyDbContext context;
        private IAdvertisementService service;
        private IFormFile mockedFile;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ShoplifyDbContext>()
                .UseInMemoryDatabase(databaseName: "ads")
                .Options;

            this.context = new ShoplifyDbContext(options);

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqIFormFile = new Mock<IFormFile>();

            this.mockedFile = moqIFormFile.Object;

            moqCloudinaryService.Setup(x => x.UploadPictureAsync(moqIFormFile.Object, "FileName"))
                .ReturnsAsync("http://test.com");

            this.service = new AdvertisementService(context, moqCloudinaryService.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.DisposeAsync();
        }

        [Test]
        public async Task CreateAsync_WithValidData_ShouldCreateSuccessfully()
        {
            var advertisement = new AdvertisementCreateServiceModel()
            {
                Name = "OnePlus 7 Pro",
                Description = "cool phone for everyday use, excellent performance",
                Price = 800,
                Condition = ProductCondition.New,
                CategoryId = "testCategoryId",
                SubCategoryId = "testCategoryId",
                Images = new List<IFormFile>
                {
                    mockedFile
                },
                TownId = "testTownId",
                Address = "str nqkoq",
                Number = "telefonce"
            };

            await service.CreateAsync(advertisement);

            var actualCategoryCount = context.Advertisements.Count();
            var expectedCategoryCount = 1;

            Assert.AreEqual(expectedCategoryCount, actualCategoryCount);
        }

        [Test]
        public async Task GetByCategoryIdAsync_WithInvalidCategoryId_ShouldReturnEmptyCollection()
        {
            var advertisement = new AdvertisementCreateServiceModel()
            {
                Name = "OnePlus 7 Pro",
                Description = "cool phone for everyday use, excellent performance",
                Price = 800,
                Condition = ProductCondition.New,
                CategoryId = "Electronics",
                SubCategoryId = "Phone",
                Images = new List<IFormFile>
                {
                    mockedFile
                },
                TownId = "testTownId",
                Address = "str nqkoq",
                Number = "telefonce"
            };

            await service.CreateAsync(advertisement);

            var ads = await service.GetAllByCategoryIdAsync("testCategoryId");

            var actualResult = ads.Count();
            var expectedResult = 0;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase("Electronics")]
        [TestCase("Phone")]
        public async Task GetByCategoryIdAsync_WithValidCategoryId_ShouldReturnCorrectly(string categoryId)
        {
            var advertisement = new AdvertisementCreateServiceModel()
            {
                Name = "OnePlus 7 Pro",
                Description = "cool phone for everyday use, excellent performance",
                Price = 800,
                Condition = ProductCondition.New,
                CategoryId = "Electronics",
                SubCategoryId = "Phone",
                Images = new List<IFormFile>
                {
                    mockedFile
                },
                TownId = "testTownId",
                Address = "str nqkoq",
                Number = "telefonce"
            };

            await service.CreateAsync(advertisement);

            var ads = await service.GetAllByCategoryIdAsync(categoryId);

            var actualResult = ads.Count();
            var expectedResult = 1;

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
