namespace Shoplify.Tests.ServicesTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using Shoplify.Domain;
    using Shoplify.Services.Implementations;
    using Shoplify.Services.Models;
    using Shoplify.Web.Data;

    [TestFixture]
    public class SubCategoryServiceTests
    {
        private ShoplifyDbContext context;
        private SubCategoryService service;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ShoplifyDbContext>()
                .UseInMemoryDatabase(databaseName: "categories")
                .Options;

            this.context = new ShoplifyDbContext(options);

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            this.service = new SubCategoryService(context, new CategoryService(context));
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.DisposeAsync();
        }

        [Test]
        public async Task CreateAsync_WithInvalidName_ShouldThrowArgumentNullException()
        {
            var subCategoryServiceModel = new SubCategoryServiceModel()
            {
                Name = null,
            };

            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateAsync(subCategoryServiceModel));
        }

        [Test]
        public async Task CreateAsync_WithInvalidCategoryId_ShouldThrowArgumentNullException()
        {
            var subCategoryServiceModel = new SubCategoryServiceModel()
            {
                Name = "test",
                CategoryId = "invalidId",
            };

            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateAsync(subCategoryServiceModel));
        }

        [Test]
        public async Task CreateAsync_WithValidData_ShouldCreateSuccessfully()
        {
            var category = new Category
            {
                Name = "Test",
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            var subCategory = new SubCategoryServiceModel()
            {
                Name = "Test Subcategory",
                CategoryId = category.Id,
            };

            var result = await service.CreateAsync(subCategory);

            var actualCategoryCount = context.Categories.Count();
            var expectedCategoryCount = 1;

            Assert.True(result);
            Assert.AreEqual(expectedCategoryCount, actualCategoryCount);
        }
    }
}
