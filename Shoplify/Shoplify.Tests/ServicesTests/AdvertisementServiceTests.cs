namespace Shoplify.Tests.ServicesTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using NUnit.Framework;
    using Shoplify.Domain.Enums;
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

            var ads = await service.GetByCategoryIdAsync("testCategoryId", 1, 1);

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

            var ads = await service.GetByCategoryIdAsync(categoryId, 1, 1);

            var actualResult = ads.Count();
            var expectedResult = 1;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase("Electronics", 1)]
        [TestCase("Electronics", 2)]
        public async Task GetByCategoryIdAsync_WithValidCategoryId_ShouldReturnCorrectlyPages(string categoryId, int page)
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

            var advertisement2 = new AdvertisementCreateServiceModel()
            {
                Name = "Phone",
                Description = "cool phone for everyday use, excellent performance",
                Price = 100,
                Condition = ProductCondition.New,
                CategoryId = "Electronics",
                SubCategoryId = "Chair",
                Images = new List<IFormFile>
                {
                    mockedFile
                },
                TownId = "testTownId",
                Address = "str nqkoq",
                Number = "telefonce"
            };

            var adsPerPage = 1;

            await service.CreateAsync(advertisement);
            await service.CreateAsync(advertisement2);

            var ads = await service.GetByCategoryIdAsync(categoryId, page, adsPerPage);

            var actualResult = ads.Count();
            var expectedResult = 1;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase("OneTwo")]
        [TestCase("PlusMinus")]
        [TestCase("7dssffsfPro")]
        [TestCase("jofifjfjdo")]
        [TestCase("OnePLUSs7 PRO")]
        public async Task GetBySearchAsync_SearchWithNoResults_ShouldReturnEmptyCollection(string search)
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

            var ads = await service.GetBySearchAsync(search, 1, 5);

            var actualResult = ads.Count();
            var expectedResult = 0;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase("One")]
        [TestCase("Plus")]
        [TestCase("7 Pro")]
        [TestCase("OnePlus")]
        [TestCase("OnePLUS 7 PRO")]
        public async Task GetBySearchAsync_WithValidSearch_ShouldReturnCorrectly(string search)
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

            var ads = await service.GetBySearchAsync(search, 1, 5);

            var actualResult = ads.Count();
            var expectedResult = 1;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase("OnePlus", 1)]
        [TestCase("OnePlus", 2)]
        public async Task GetBySearchAsync_WithValidSearch_ShouldReturnCorrectlyPages(string categoryId, int page)
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

            var advertisement2 = new AdvertisementCreateServiceModel()
            {
                Name = "OnePlus 6T",
                Description = "cool phone for everyday use, excellent performance",
                Price = 100,
                Condition = ProductCondition.New,
                CategoryId = "Electronics",
                SubCategoryId = "Chair",
                Images = new List<IFormFile>
                {
                    mockedFile
                },
                TownId = "testTownId",
                Address = "str nqkoq",
                Number = "telefonce"
            };

            var adsPerPage = 1;

            await service.CreateAsync(advertisement);
            await service.CreateAsync(advertisement2);

            var ads = await service.GetBySearchAsync(categoryId, page, adsPerPage);

            var actualResult = ads.Count();
            var expectedResult = 1;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task GetLatestAsync_WithNoAds_ShouldReturnEmptyCollection()
        {
            var ads = await service.GetLatestAsync(4, "test");

            var actualResult = ads.Count();
            var expectedResult = 0;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task GetLatestAsync_WithAds_ShouldReturnCorrectly()
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

            var advertisement2 = new AdvertisementCreateServiceModel()
            {
                Name = "OnePlus 6T",
                Description = "cool phone for everyday use, excellent performance",
                Price = 100,
                Condition = ProductCondition.New,
                CategoryId = "Electronics",
                SubCategoryId = "Chair",
                Images = new List<IFormFile>
                {
                    mockedFile
                },
                TownId = "testTownId",
                Address = "str nqkoq",
                Number = "telefonce"
            };

            await service.CreateAsync(advertisement);
            await service.CreateAsync(advertisement2);

            var expectedResult = 2;

            var ads = await service.GetLatestAsync(expectedResult, "test");

            var actualResult = ads.Count();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task GetCountByCategoryIdAsync_WithNoAds_ShouldReturnCorrectly()
        {
            var expectedResult = 0;

            var actualResult = await service.GetCountByCategoryIdAsync("Electronics");

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task GetCountByCategoryIdAsync_WithAds_ShouldReturnCorrectly()
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

            var advertisement2 = new AdvertisementCreateServiceModel()
            {
                Name = "OnePlus 6T",
                Description = "cool phone for everyday use, excellent performance",
                Price = 100,
                Condition = ProductCondition.New,
                CategoryId = "Electronics",
                SubCategoryId = "Chair",
                Images = new List<IFormFile>
                {
                    mockedFile
                },
                TownId = "testTownId",
                Address = "str nqkoq",
                Number = "telefonce"
            };

            await service.CreateAsync(advertisement);
            await service.CreateAsync(advertisement2);

            var expectedResult = 2;

            var actualResult = await service.GetCountByCategoryIdAsync("Electronics");

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task GetCountBySearchAsync_WithNoAds_ShouldReturnCorrectly()
        {
            var expectedResult = 0;

            var actualResult = await service.GetCountBySearchAsync("Electronics");

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task GetCountBySearchAsync_WithAds_ShouldReturnCorrectly()
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

            var advertisement2 = new AdvertisementCreateServiceModel()
            {
                Name = "OnePlus 6T",
                Description = "cool phone for everyday use, excellent performance",
                Price = 100,
                Condition = ProductCondition.New,
                CategoryId = "Electronics",
                SubCategoryId = "Chair",
                Images = new List<IFormFile>
                {
                    mockedFile
                },
                TownId = "testTownId",
                Address = "str nqkoq",
                Number = "telefonce"
            };

            await service.CreateAsync(advertisement);
            await service.CreateAsync(advertisement2);

            var expectedResult = 2;

            var actualResult = await service.GetCountBySearchAsync("OnePlus");

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
