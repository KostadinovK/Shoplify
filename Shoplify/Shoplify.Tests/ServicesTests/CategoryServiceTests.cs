using Shoplify.Domain;

namespace Shoplify.Tests.ServicesTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using Shoplify.Services.Implementations;
    using Shoplify.Services.Models;
    using Shoplify.Web.Data;

    [TestFixture]
    public class CategoryServiceTests
    {
        private ShoplifyDbContext context;
        private CategoryService service;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ShoplifyDbContext>()
                .UseInMemoryDatabase(databaseName: "categories")
                .Options;

            this.context = new ShoplifyDbContext(options);

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            this.service = new CategoryService(context);
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.DisposeAsync();
        }

        [Test]
        public async Task CreateAsync_WithInvalidName_ShouldThrowArgumentNullException()
        {
            var category = new CategoryServiceModel
            {
                Name = null,
                CssIconClass = null
            };

            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateAsync(category));
        }

        [Test]
        public async Task CreateAsync_WithValidData_ShouldCreateSuccessfully()
        {
            var category = new CategoryServiceModel
            {
                Name = "Test",
                CssIconClass = "test"
            };

            var result = await service.CreateAsync(category);

            var actualCategoryCount = context.Categories.Count();
            var expectedCategoryCount = 1;

            Assert.True(result);
            Assert.AreEqual(expectedCategoryCount, actualCategoryCount);
        }

        [Test]
        public async Task CreateAllAsync_WithNullNames_ShouldThrowArgumentNullException()
        {
            List<string> names = null;

            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateAllAsync(names));
        }

        [Test]
        public async Task CreateAllAsync_WithInvalidIconsCount_ShouldThrowArgumentNullException()
        {
            var names = new List<string>{ "Test", "Test2"};
            var icons = new List<string>{"test"};

            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateAllAsync(names, icons));
        }

        [Test]
        public async Task CreateAllAsync_WithValidNamesWithoutIcons_ShouldCreateSuccessfully()
        {
            var names = new List<string>() { "home", "test" };

            await service.CreateAllAsync(names);

            var actualCategoryCount = context.Categories.Count();
            var expectedCategoryCount = 2;
            var categories = context.Categories.ToList();
            Assert.AreEqual(expectedCategoryCount, actualCategoryCount);
        }

        [Test]
        public async Task CreateAllAsync_WithValidNamesAndIcons_ShouldCreateSuccessfully()
        {
            var names = new List<string>() { "home", "test" };
            var icons = new List<string>() { "home", "test" };

            await service.CreateAllAsync(names, icons);

            var actualCategoryCount = context.Categories.Count();
            var expectedCategoryCount = 2;

            Assert.AreEqual(expectedCategoryCount, actualCategoryCount);
        }

        [Test]
        public async Task ContainsByIdAsync_WithValidId_ShouldReturnTrue()
        {
            var categoryName = "test";

            var category = new Category
            {
                Name = categoryName
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            var categoryFromDb = await context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);

            var result = await service.ContainsByIdAsync(categoryFromDb.Id.ToString());

            Assert.IsTrue(result);
        }

        [Test]
        public async Task ContainsByIdAsync_WithInvalidId_ShouldReturnFalse()
        {
            var category = new Category
            {
                Name = "test"
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            var result = await service.ContainsByIdAsync("invalidId");

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetByIdAsync_WithInvalidId_ShouldThrowArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await service.GetByIdAsync("invalidId"));
        }

        [Test]
        public async Task GetByIdAsync_WithValidId_ShouldReturnCorrectly()
        {
            var categoryName = "test";

            var category = new Category
            {
                Name = categoryName
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            var categoryFromDb = await context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);

            var actualCategory = await service.GetByIdAsync(categoryFromDb.Id.ToString());

            var expectedCategory = new CategoryServiceModel
            {
                Name = categoryFromDb.Name,
                CssIconClass = categoryFromDb.CssIconClass,
                Id = categoryFromDb.Id,
            };

            AssertEx.PropertyValuesAreEquals(actualCategory, expectedCategory);
        }

        [Test]
        public async Task GetByNameAsync_WithInvalidName_ShouldThrowArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await service.GetByNameAsync("invalidName"));
        }

        [Test]
        public async Task GetByNameAsync_WithValidName_ShouldReturnCorrectly()
        {
            var categoryName = "test";

            var category = new Category
            {
                Name = categoryName
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            var actualCategory = await service.GetByNameAsync(category.Name);

            var expectedCategory = new CategoryServiceModel
            {
                Name = category.Name,
                CssIconClass = category.CssIconClass,
                Id = category.Id,
            };

            AssertEx.PropertyValuesAreEquals(actualCategory, expectedCategory);
        }

        [Test]
        public async Task GetAll_ShouldReturnCorrectly()
        {
            var categoryName = "test";
            var categoryName2 = "test2";

            var category = new Category
            {
                Name = categoryName
            };

            var category2 = new Category
            {
                Name = categoryName2
            };

            await context.Categories.AddAsync(category);
            await context.Categories.AddAsync(category2);

            await context.SaveChangesAsync();

            var categories = service.GetAll().ToList();

            var expectedCategory = new CategoryServiceModel
            {
                Name = category.Name,
                CssIconClass = category.CssIconClass,
                Id = category.Id,
            };

            var expectedCategory2 = new CategoryServiceModel
            {
                Name = category2.Name,
                CssIconClass = category2.CssIconClass,
                Id = category2.Id,
            };

            var expectedCategoriesCount = 2;
            var actualCategoriesCount = categories.Count;

            Assert.AreEqual(expectedCategoriesCount, actualCategoriesCount);
            AssertEx.PropertyValuesAreEquals(categories[0], expectedCategory);
            AssertEx.PropertyValuesAreEquals(categories[1], expectedCategory2);
        }
    }
}
