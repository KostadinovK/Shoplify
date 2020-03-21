using Shoplify.Common;
using Shoplify.Services.Models.Advertisement;

namespace Shoplify.Tests.ServicesTests
{
    using System;
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
        public async Task EditAsync_WithValidData_ShouldEditSuccessfully()
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

            var adFromDb = context.Advertisements.SingleOrDefault(a => a.Name == "OnePlus 7 Pro");

            var editedAd = new AdvertisementEditServiceModel()
            {
                Id = adFromDb.Id,
                UserId = adFromDb.UserId,
                Name = "OnePlus 5",
                Description = adFromDb.Description,
                Price = 500,
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

            await service.EditAsync(editedAd);

            var actualAdName = adFromDb.Name;
            var expectedAdName = "OnePlus 5";

            Assert.AreEqual(expectedAdName, actualAdName);
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

            var ads = await service.GetByCategoryIdAsync("testCategoryId", 1, 1, "test");

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

            var ads = await service.GetByCategoryIdAsync(categoryId, 1, 1, "test");

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

            var ads = await service.GetByCategoryIdAsync(categoryId, page, adsPerPage, "test");

            var actualResult = ads.Count();
            var expectedResult = 1;

            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public async Task GetByUserIdAsync_WithInvalidUserId_ShouldReturnEmptyCollection()
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
                Number = "telefonce",
                UserId = "invalid"
            };

            await service.CreateAsync(advertisement);

            var ads = await service.GetByUserIdAsync("valid", 1, 1, "test");

            var actualResult = ads.Count();
            var expectedResult = 0;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task GetByUserIdAsync_WithValidUserId_ShouldReturnCorrectly()
        {
            var userId = "valid";

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
                Number = "telefonce",
                UserId = userId
            };

            await service.CreateAsync(advertisement);

            var ads = await service.GetByUserIdAsync(userId, 1, 1, "test");

            var actualResult = ads.Count();
            var expectedResult = 1;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase("valid", 1)]
        [TestCase("valid", 2)]
        public async Task GetByUserIdAsync_WithValidUserId_ShouldReturnCorrectlyPages(string userId, int page)
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
                Number = "telefonce",
                UserId = "valid"
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
                Number = "telefonce",
                UserId = userId
            };

            var adsPerPage = 1;

            await service.CreateAsync(advertisement);
            await service.CreateAsync(advertisement2);

            var ads = await service.GetByUserIdAsync(userId, page, adsPerPage, "test");

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

            var ads = await service.GetBySearchAsync(search, 1, 5, "test");

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

            var ads = await service.GetBySearchAsync(search, 1, 5, "test");

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

            var ads = await service.GetBySearchAsync(categoryId, page, adsPerPage, "test");

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
        public async Task GetCountByUserIdAsync_WithNoAds_ShouldReturnCorrectly()
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
                Number = "telefonce",
                UserId = "valid"
            };

            await service.CreateAsync(advertisement);

            var expectedResult = 0;

            var actualResult = await service.GetCountByUserIdAsync("test");

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task GetCountByUserIdAsync_WithAds_ShouldReturnCorrectly()
        {
            var userId = "valid";

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
                Number = "telefonce",
                UserId = userId
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
                Number = "telefonce",
                UserId = userId
            };

            await service.CreateAsync(advertisement);
            await service.CreateAsync(advertisement2);

            var expectedResult = 2;

            var actualResult = await service.GetCountByUserIdAsync(userId);

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

        [Test]
        public async Task Contains_WithInvalidId_ShouldReturnFalse()
        {
            var result = service.Contains("invalid");

            Assert.IsFalse(result);
        }

        [Test]
        public async Task Contains_WithValidId_ShouldReturnTrue()
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

            var ad = await context.Advertisements.FirstOrDefaultAsync();

            var result = service.Contains(ad.Id);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetByIdAsync_WithInvalidId_ShouldThrowArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await service.GetByIdAsync("invalid"));
        }

        [Test]
        public async Task GetByIdAsync_WithValidId_ShouldReturnCorrectly()
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

            var ad = await context.Advertisements.FirstOrDefaultAsync();

            var result = await service.GetByIdAsync(ad.Id);

            Assert.IsNotNull(result);
        }

        [Test]
        public void ArchiveByIdAsync_WithInvalidAdId_ShouldThrowArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await service.ArchiveByIdAsync("invalid"));
        }

        [Test]
        public async Task ArchiveByIdAsync_ShouldWorkCorrectly()
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

            var ad = await context.Advertisements.FirstOrDefaultAsync();

            await service.ArchiveByIdAsync(ad.Id);

            var expectedArchivedDate = DateTime.UtcNow.Date;
            var actualArchivedDate = ad.ArchivedOn.GetValueOrDefault().Date;

            Assert.IsTrue(ad.IsArchived);
            Assert.AreEqual(expectedArchivedDate, actualArchivedDate);
        }

        [Test]
        public void UnarchiveByIdAsync_WithInvalidAdId_ShouldThrowArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await service.UnarchiveByIdAsync("invalid"));
        }

        [Test]
        public async Task UnarchiveByIdAsync_ShouldWorkCorrectly()
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
                Number = "telefonce",
                IsArchived = true,
                ArchivedOn = DateTime.UtcNow,
            };

            await service.CreateAsync(advertisement);

            var ad = await context.Advertisements.FirstOrDefaultAsync();

            await service.UnarchiveByIdAsync(ad.Id);

            var expectedArchivedDate = DateTime.UtcNow.AddDays(GlobalConstants.AdvertisementDurationDays).Date;
            var actualArchivedDate = ad.ArchivedOn.GetValueOrDefault().Date;

            Assert.IsFalse(ad.IsArchived);
            Assert.AreEqual(expectedArchivedDate, actualArchivedDate);
        }

        [Test]
        public void PromoteByIdAsync_WithInvalidAdId_ShouldThrowArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await service.PromoteByIdAsync("invalid", 7));
        }

        [TestCase(1)]
        [TestCase(7)]
        [TestCase(14)]
        [TestCase(30)]
        public async Task PromoteByIdAsync_ShouldWorkCorrectly(int days)
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
                Number = "telefonce"
            };

            await service.CreateAsync(advertisement);

            var ad = await context.Advertisements.FirstOrDefaultAsync();

            await service.PromoteByIdAsync(ad.Id, days);

            var expectedPromoteUntilDate = DateTime.UtcNow.Date.AddDays(days);
            var actualPromotedUntilDate = ad.PromotedUntil.GetValueOrDefault().Date;

            Assert.IsTrue(ad.IsPromoted);
            Assert.AreEqual(expectedPromoteUntilDate, actualPromotedUntilDate);
        }

        [Test]
        public void UnpromoteByIdAsync_WithInvalidAdId_ShouldThrowArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await service.UnpromoteByIdAsync("invalid"));
        }

        [TestCase(1)]
        [TestCase(7)]
        [TestCase(14)]
        [TestCase(30)]
        public async Task UnpromoteByIdAsync_ShouldWorkCorrectly(int days)
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
                Number = "telefonce"
            };

            await service.CreateAsync(advertisement);

            var ad = await context.Advertisements.FirstOrDefaultAsync();

            await service.PromoteByIdAsync(ad.Id, days);

            await service.UnpromoteByIdAsync(ad.Id);

            Assert.IsFalse(ad.IsPromoted);
        }

        [Test]
        public void BanByIdAsync_WithInvalidAdId_ShouldThrowArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await service.BanByIdAsync("invalid"));
        }

        [Test]
        public async Task BanByIdAsync_ShouldWorkCorrectly()
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

            var ad = await context.Advertisements.FirstOrDefaultAsync();

            await service.BanByIdAsync(ad.Id);

            var expectedArchivedDate = DateTime.UtcNow.Date;
            var actualArchivedDate = ad.BannedOn.GetValueOrDefault().Date;

            Assert.IsTrue(ad.IsBanned);
            Assert.AreEqual(expectedArchivedDate, actualArchivedDate);
        }

        [Test]
        public void UnbanByIdAsync_WithInvalidAdId_ShouldThrowArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await service.UnbanByIdAsync("invalid"));
        }

        [Test]
        public async Task UnbanByIdAsync_ShouldWorkCorrectly()
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
                Number = "telefonce",
            };

            await service.CreateAsync(advertisement);

            var ad = await context.Advertisements.FirstOrDefaultAsync();

            await service.BanByIdAsync(ad.Id);

            await service.UnbanByIdAsync(ad.Id);

            Assert.IsFalse(ad.IsBanned);
            Assert.IsNull(ad.BannedOn);
        }

        [Test]
        public void IncrementViewsAsync_WithInvalidAdId_ShouldThrowArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await service.IncrementViewsAsync("invalid"));
        }

        [Test]
        public async Task IncrementViewsAsync_WithValidAdId_ShouldIncrementCorrectly()
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

            var ad = await context.Advertisements.FirstOrDefaultAsync();

            await service.IncrementViewsAsync(ad.Id);

            var expectedViewsCount = 1;
            var actualViewCount = ad.Views;

            Assert.AreEqual(expectedViewsCount, actualViewCount);
        }

        [Test]
        public async Task GetBannedAdsByUserIdAsync_WithValidId_ShouldReturnCorrectly()
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
                Number = "telefonce",
                UserId = "test"
            };

            await service.CreateAsync(advertisement);

            var ad = context.Advertisements.SingleOrDefault(a => a.UserId == "test");

            ad.IsBanned = true;

            await context.SaveChangesAsync();

            var ads = await service.GetBannedAdsByUserIdAsync("test", 1);

            var expectedCount = 1;
            var actualCount = ads.Count();

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetBannedAdsCountByUserIdAsync_WithValidId_ShouldReturnCorrectly()
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

            await service.CreateAsync(advertisement);

            var ad = context.Advertisements.SingleOrDefault(a => a.UserId == "test");

            ad.IsBanned = true;

            await context.SaveChangesAsync();

            var count = await service.GetBannedAdsCountByUserIdAsync("test");

            var expectedCount = 1;
            var actualCount = count;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetArchivedAdsByUserIdAsync_WithValidId_ShouldReturnCorrectly()
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

            await service.CreateAsync(advertisement);

            var ad = context.Advertisements.SingleOrDefault(a => a.UserId == "test");

            ad.IsArchived = true;

            await context.SaveChangesAsync();

            var ads = await service.GetArchivedAdsByUserIdAsync("test", 1);

            var expectedCount = 1;
            var actualCount = ads.Count();

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetArchivedAdsCountByUserIdAsync_WithValidId_ShouldReturnCorrectly()
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

            await service.CreateAsync(advertisement);

            var ad = context.Advertisements.SingleOrDefault(a => a.UserId == "test");

            ad.IsArchived = true;

            await context.SaveChangesAsync();

            var count = await service.GetArchivedAdsCountByUserIdAsync("test");

            var expectedCount = 1;
            var actualCount = count;

            Assert.AreEqual(expectedCount, actualCount);
        }


        [Test]
        public async Task ArchiveAllExpiredAdsAsync_WithNoExpiredAd_ShouldNotArchive()
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

            var date = DateTime.UtcNow;

            await service.CreateAsync(advertisement);

            var ad = context.Advertisements.SingleOrDefault(a => a.UserId == "test");

            ad.ArchivedOn = date.AddDays(7);

            await context.SaveChangesAsync();

            var actualCount = await service.ArchiveAllExpiredAdsAsync(date);

            var expectedCount = 0;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task ArchiveAllExpiredAdsAsync_WithExpiredAd_ShouldArchive()
        {
            var advertisement = new AdvertisementCreateServiceModel()
            {
                Name = "ad1",
                Description = "cool phone for everyday use, excellent performance",
                Price = 800,
                Condition = ProductCondition.New,
                CategoryId = "Electronics",
                SubCategoryId = "Phone",
                TownId = "testTownId",
                Address = "str nqkoq",
                Number = "telefonce",
                UserId = "test",
            };

            var advertisement2 = new AdvertisementCreateServiceModel()
            {
                Name = "ad2",
                Description = "cool phone for everyday use, excellent performance",
                Price = 800,
                Condition = ProductCondition.New,
                CategoryId = "Electronics",
                SubCategoryId = "Phone",
                TownId = "testTownId",
                Address = "str nqkoq",
                Number = "telefonce",
                UserId = "test",
            };

            var advertisement3 = new AdvertisementCreateServiceModel()
            {
                Name = "ad3",
                Description = "cool phone for everyday use, excellent performance",
                Price = 800,
                Condition = ProductCondition.New,
                CategoryId = "Electronics",
                SubCategoryId = "Phone",
                TownId = "testTownId",
                Address = "str nqkoq",
                Number = "telefonce",
                UserId = "test",
            };

            var date = DateTime.UtcNow;

            await service.CreateAsync(advertisement);
            await service.CreateAsync(advertisement2);
            await service.CreateAsync(advertisement3);

            var ad1 = context.Advertisements.SingleOrDefault(a => a.Name == "ad1");
            var ad2 = context.Advertisements.SingleOrDefault(a => a.Name == "ad2");
            var ad3 = context.Advertisements.SingleOrDefault(a => a.Name == "ad3");

            ad1.ArchivedOn = date.AddDays(7);
            ad2.ArchivedOn = date;
            ad3.ArchivedOn = date.AddDays(-1);

            await context.SaveChangesAsync();

            var actualCount = await service.ArchiveAllExpiredAdsAsync(date);

            var expectedCount = 2;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task UnPromoteAllExpiredAdsAsync_WithNoExpiredAd_ShouldNotUnPromote()
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

            var date = DateTime.UtcNow;

            await service.CreateAsync(advertisement);

            var ad = context.Advertisements.SingleOrDefault(a => a.UserId == "test");

            ad.PromotedUntil = date.AddDays(7);

            await context.SaveChangesAsync();

            var actualCount = await service.UnPromoteAllExpiredAdsAsync(date);

            var expectedCount = 0;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task UnPromoteAllExpiredAdsAsync_WithExpiredAd_ShouldUnPromote()
        {
            var advertisement = new AdvertisementCreateServiceModel()
            {
                Name = "ad1",
                Description = "cool phone for everyday use, excellent performance",
                Price = 800,
                Condition = ProductCondition.New,
                CategoryId = "Electronics",
                SubCategoryId = "Phone",
                TownId = "testTownId",
                Address = "str nqkoq",
                Number = "telefonce",
                UserId = "test",
            };

            var advertisement2 = new AdvertisementCreateServiceModel()
            {
                Name = "ad2",
                Description = "cool phone for everyday use, excellent performance",
                Price = 800,
                Condition = ProductCondition.New,
                CategoryId = "Electronics",
                SubCategoryId = "Phone",
                TownId = "testTownId",
                Address = "str nqkoq",
                Number = "telefonce",
                UserId = "test",
            };

            var advertisement3 = new AdvertisementCreateServiceModel()
            {
                Name = "ad3",
                Description = "cool phone for everyday use, excellent performance",
                Price = 800,
                Condition = ProductCondition.New,
                CategoryId = "Electronics",
                SubCategoryId = "Phone",
                TownId = "testTownId",
                Address = "str nqkoq",
                Number = "telefonce",
                UserId = "test",
            };

            var date = DateTime.UtcNow;

            await service.CreateAsync(advertisement);
            await service.CreateAsync(advertisement2);
            await service.CreateAsync(advertisement3);

            var ad1 = context.Advertisements.SingleOrDefault(a => a.Name == "ad1");
            var ad2 = context.Advertisements.SingleOrDefault(a => a.Name == "ad2");
            var ad3 = context.Advertisements.SingleOrDefault(a => a.Name == "ad3");

            ad1.IsPromoted = true;
            ad1.PromotedUntil = date.AddDays(7);

            ad2.IsPromoted = true;
            ad2.PromotedUntil = date;

            ad3.IsPromoted = true;
            ad3.PromotedUntil = date.AddDays(-1);

            await context.SaveChangesAsync();

            var actualCount = await service.UnPromoteAllExpiredAdsAsync(date);

            var expectedCount = 2;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllAdsCountAsync_WithNoAds_ShouldReturnCorrectly()
        {
            var actualCount = await service.GetAllAdsCountAsync();
            var expectedCount = 0;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllAdsCountAsync_WithAds_ShouldReturnCorrectly()
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

            var advertisement2 = new AdvertisementCreateServiceModel()
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

            await service.CreateAsync(advertisement);
            await service.CreateAsync(advertisement2);

            var actualCount = await service.GetAllAdsCountAsync();
            var expectedCount = 2;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllAdsAsync_WithNoAds_ShouldReturnCorrectly()
        {
            var ads = await service.GetAllAdsAsync(1, 10);

            var actualCount = ads.Count();
            var expectedCount = 0;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllAdsAsync_WithAds_ShouldReturnCorrectly()
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

            var advertisement2 = new AdvertisementCreateServiceModel()
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

            await service.CreateAsync(advertisement);
            await service.CreateAsync(advertisement2);

            var ads = await service.GetAllAdsAsync(1, 10);

            var actualCount = ads.Count();
            var expectedCount = 2;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllAdsAsync_WithAds_PaginationShouldWorkCorrectly()
        {
            var adsPerPage = 1;
            var page = 1;

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

            var advertisement2 = new AdvertisementCreateServiceModel()
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

            await service.CreateAsync(advertisement);
            await service.CreateAsync(advertisement2);

            var ads = await service.GetAllAdsAsync(page, adsPerPage);

            var actualCount = ads.Count();
            var expectedCount = 1;

            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
