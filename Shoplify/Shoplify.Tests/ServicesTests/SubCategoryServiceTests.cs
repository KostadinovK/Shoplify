using System.Collections.Generic;

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

        [Test]
        public async Task CreateAllAsync_WithNullNames_ShouldThrowArgumentNullException()
        {
            List<string> names = null;

            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateAllAsync(names, "test"));
        }

        [Test]
        public async Task CreateAllAsync_WithValidNamesWithoutIcons_ShouldCreateSuccessfully()
        {
            var category = new Category
            {
                Name = "Test",
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            var names = new List<string>() { "test1", "test2" };

            await service.CreateAllAsync(names, category.Id);

            var actualCategoryCount = context.SubCategories.Count();
            var expectedCategoryCount = 2;

            Assert.AreEqual(expectedCategoryCount, actualCategoryCount);
        }

        [Test]
        public async Task ContainsByIdAsync_WithValidId_ShouldReturnTrue()
        {
            var subCategoryName = "test";

            var subCategory = new SubCategory()
            {
                Name = subCategoryName,
                CategoryId = "test",
            };

            await context.SubCategories.AddAsync(subCategory);
            await context.SaveChangesAsync();

            var subCategoryFromDb = await context.SubCategories.FirstOrDefaultAsync(s => s.Name == subCategoryName);

            var result = await service.ContainsByIdAsync(subCategoryFromDb.Id.ToString());

            Assert.IsTrue(result);
        }

        [Test]
        public async Task ContainsByIdAsync_WithInvalidId_ShouldReturnFalse()
        {
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
            var subCategoryName = "test";

            var subCategory = new SubCategory()
            {
                Name = subCategoryName,
                CategoryId = "test",
            };

            await context.SubCategories.AddAsync(subCategory);
            await context.SaveChangesAsync();

            var subCategoryFromDb = await context.SubCategories.FirstOrDefaultAsync(s => s.Name == subCategoryName);

            var actualCategory = await service.GetByIdAsync(subCategoryFromDb.Id.ToString());

            var expectedCategory = new SubCategoryServiceModel()
            {
                Name = subCategory.Name,
                CategoryId = subCategory.CategoryId,
                Id = subCategory.Id,
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
            var subCategoryName = "test";

            var subCategory = new SubCategory()
            {
                Name = subCategoryName,
                CategoryId = "test",
            };

            await context.SubCategories.AddAsync(subCategory);
            await context.SaveChangesAsync();

            var actualCategory = await service.GetByNameAsync(subCategory.Name);

            var expectedCategory = new SubCategoryServiceModel()
            {
                Name = subCategory.Name,
                CategoryId = subCategory.CategoryId,
                Id = subCategory.Id,
            };

            AssertEx.PropertyValuesAreEquals(actualCategory, expectedCategory);
        }

        [Test]
        public async Task GetAllByCategoryId_ShouldReturnCorrectly()
        {
            var categoryName = "test";

            var category = new Category
            {
                Name = categoryName,
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            var subCategoryOne = new SubCategory
            {
                Name = "test",
                CategoryId = category.Id,
            };

            var subCategoryTwo = new SubCategory
            {
                Name = "test2",
                CategoryId = category.Id,
            };

            var subCategoryThree = new SubCategory
            {
                Name = "test3",
                CategoryId = "anotherId",
            };

            await context.SubCategories.AddAsync(subCategoryOne);
            await context.SubCategories.AddAsync(subCategoryTwo);
            await context.SubCategories.AddAsync(subCategoryThree);
            await context.SaveChangesAsync();

            var subCategories = service.GetAllByCategoryId(category.Id).ToList();

            var expectedSubCategoryOne = new SubCategoryServiceModel()
            {
                Name = subCategoryOne.Name,
                CategoryId = subCategoryOne.CategoryId,
                Id = subCategoryOne.Id,
            };

            var expectedSubCategoryTwo = new SubCategoryServiceModel
            {
                Name = subCategoryTwo.Name,
                CategoryId = subCategoryTwo.CategoryId,
                Id = subCategoryTwo.Id,
            };

            var expectedCategoriesCount = 2;
            var actualCategoriesCount = subCategories.Count;

            Assert.AreEqual(expectedCategoriesCount, actualCategoriesCount);
            AssertEx.PropertyValuesAreEquals(subCategories[0], expectedSubCategoryOne);
            AssertEx.PropertyValuesAreEquals(subCategories[1], expectedSubCategoryTwo);
        }
    }
}
