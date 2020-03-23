using System;
using Microsoft.AspNetCore.Http;
using Moq;
using Shoplify.Domain;

namespace Shoplify.Tests.ServicesTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using Shoplify.Services.Implementations;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models.Report;
    using Shoplify.Web.Data;

    [TestFixture]
    public class ReportServiceTests
    {
        private ShoplifyDbContext context;
        private IReportService service;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ShoplifyDbContext>()
                .UseInMemoryDatabase(databaseName: "reports")
                .Options;

            this.context = new ShoplifyDbContext(options);

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var moqCloudinaryService = new Mock<ICloudinaryService>();

            this.service = new ReportService(context, new AdvertisementService(context, moqCloudinaryService.Object));
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.DisposeAsync();
        }

        [Test]
        public async Task CreateAsync_WithValidData_ShouldCreateSuccessfully()
        {
            var report = new ReportCreateServiceModel()
            {
                Description = "cool report description",
                ReportedAdvertisementId = "test",
                ReportedUserId = "testUser",
                ReportingUserId = "test123",
            };

            await service.CreateAsync(report);

            var actualCategoryCount = context.Reports.Count();
            var expectedCategoryCount = 1;

            Assert.AreEqual(expectedCategoryCount, actualCategoryCount);
        }

        [Test]
        public async Task ApproveByIdAsync_WithInvalidReportId_ShouldReturnFalse()
        {
            var reportId = "invalid";

            var result = await service.ApproveByIdAsync(reportId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task ApproveByIdAsync_WithValidReportId_ShouldReturnTrue()
        {
            var reportedUserId = "user";

            var ad = new Advertisement
            {
                Name = "test",
                Price = 200,
                Description = "test",
                UserId = reportedUserId,
                CategoryId = "category",
                SubCategoryId = "subCategory",
                TownId = "town",
                CreatedOn = DateTime.UtcNow,
                Address = "test",
                Number = "test"
            };

            await context.Advertisements.AddAsync(ad);
            await context.SaveChangesAsync();

            var adFromDb = await context.Advertisements.FirstOrDefaultAsync();

            var report = new Report
            {
                ReportedAdvertisementId = adFromDb.Id,
                ReportingUserId = "reporting",
                ReportedUserId = reportedUserId,
                Description = "test"
            };

            await context.Reports.AddAsync(report);
            await context.SaveChangesAsync();

            var result = await service.ApproveByIdAsync(report.Id);

            Assert.IsTrue(result);
            Assert.IsTrue(report.IsArchived);
            Assert.IsTrue(adFromDb.IsBanned);
        }

        [Test]
        public async Task RejectByIdAsync_WithInvalidReportId_ShouldReturnFalse()
        {
            var reportId = "invalid";

            var result = await service.RejectByIdAsync(reportId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task RejectByIdAsync_WithValidReportId_ShouldReturnTrue()
        {
            var reportedUserId = "user";

            var ad = new Advertisement
            {
                Name = "test",
                Price = 200,
                Description = "test",
                UserId = reportedUserId,
                CategoryId = "category",
                SubCategoryId = "subCategory",
                TownId = "town",
                CreatedOn = DateTime.UtcNow,
                Address = "test",
                Number = "test"
            };

            await context.Advertisements.AddAsync(ad);
            await context.SaveChangesAsync();

            var adFromDb = await context.Advertisements.FirstOrDefaultAsync();

            var report = new Report
            {
                ReportedAdvertisementId = adFromDb.Id,
                ReportingUserId = "reporting",
                ReportedUserId = reportedUserId,
                Description = "test"
            };

            await context.Reports.AddAsync(report);
            await context.SaveChangesAsync();

            var result = await service.RejectByIdAsync(report.Id);

            Assert.IsTrue(result);
            Assert.IsTrue(report.IsArchived);
            Assert.IsFalse(adFromDb.IsBanned);
        }

        [Test]
        public async Task ArchiveByIdAsync_WithInvalidReportId_ShouldReturnFalse()
        {
            var reportId = "invalid";

            var result = await service.ArchiveByIdAsync(reportId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task ArchiveByIdAsync_WithValidReportId_ShouldReturnTrue()
        {
            var report = new Report
            {
                ReportedAdvertisementId = "ad",
                ReportingUserId = "reporting",
                ReportedUserId = "reported",
                Description = "test"
            };

            await context.Reports.AddAsync(report);
            await context.SaveChangesAsync();

            var result = await service.ArchiveByIdAsync(report.Id);

            Assert.IsTrue(result);
            Assert.IsTrue(report.IsArchived);
        }

        [Test]
        public async Task GetAllUnArchivedCountAsync_WithNoReports_ShouldReturnCorrectly()
        {
            var actualCount = await service.GetAllUnArchivedCountAsync();
            var expectedCount = 0;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllAdsCountAsync_WithAds_ShouldReturnCorrectly()
        {
            var report = new Report
            {
                ReportedAdvertisementId = "ad",
                ReportingUserId = "reporting",
                ReportedUserId = "reported",
                Description = "test"
            };

            var report2 = new Report
            {
                ReportedAdvertisementId = "ad",
                ReportingUserId = "reporting",
                ReportedUserId = "reported",
                Description = "test"
            };

            var archivedReport = new Report
            {
                ReportedAdvertisementId = "ad",
                ReportingUserId = "reporting",
                ReportedUserId = "reported",
                Description = "test",
                IsArchived = true
            };

            await context.Reports.AddAsync(report);
            await context.Reports.AddAsync(report2);
            await context.Reports.AddAsync(archivedReport);

            await context.SaveChangesAsync();

            var actualCount = await service.GetAllUnArchivedCountAsync();
            var expectedCount = 2;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllUnArchivedAsync_WithNoReports_ShouldReturnCorrectly()
        {
            var reports = await service.GetAllUnArchivedAsync(1, 10);

            var actualCount = reports.Count();
            var expectedCount = 0;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllUnArchivedAsync_WithReports_ShouldReturnCorrectly()
        {
            var report = new Report
            {
                ReportedAdvertisementId = "ad",
                ReportingUserId = "reporting",
                ReportedUserId = "reported",
                Description = "test"
            };

            var report2 = new Report
            {
                ReportedAdvertisementId = "ad",
                ReportingUserId = "reporting",
                ReportedUserId = "reported",
                Description = "test"
            };

            var archivedReport = new Report
            {
                ReportedAdvertisementId = "ad",
                ReportingUserId = "reporting",
                ReportedUserId = "reported",
                Description = "test",
                IsArchived = true
            };

            await context.Reports.AddAsync(report);
            await context.Reports.AddAsync(report2);
            await context.Reports.AddAsync(archivedReport);

            await context.SaveChangesAsync();

            var reports = await service.GetAllUnArchivedAsync(1, 10);

            var actualCount = reports.Count();
            var expectedCount = 2;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllAdsAsync_WithAds_PaginationShouldWorkCorrectly()
        {
            var report = new Report
            {
                ReportedAdvertisementId = "ad",
                ReportingUserId = "reporting",
                ReportedUserId = "reported",
                Description = "test"
            };

            var report2 = new Report
            {
                ReportedAdvertisementId = "ad",
                ReportingUserId = "reporting",
                ReportedUserId = "reported",
                Description = "test"
            };

            var archivedReport = new Report
            {
                ReportedAdvertisementId = "ad",
                ReportingUserId = "reporting",
                ReportedUserId = "reported",
                Description = "test",
                IsArchived = true
            };

            await context.Reports.AddAsync(report);
            await context.Reports.AddAsync(report2);
            await context.Reports.AddAsync(archivedReport);

            await context.SaveChangesAsync();

            var reports = await service.GetAllUnArchivedAsync(1, 1);

            var actualCount = reports.Count();
            var expectedCount = 1;

            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
