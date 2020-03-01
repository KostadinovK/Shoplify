using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Shoplify.Domain.Enums;
using Shoplify.Services.Models;
using Shoplify.Services.Models.Report;

namespace Shoplify.Tests.ServicesTests
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using Shoplify.Services.Implementations;
    using Shoplify.Services.Interfaces;
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

            this.service = new ReportService(context);
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
    }
}
