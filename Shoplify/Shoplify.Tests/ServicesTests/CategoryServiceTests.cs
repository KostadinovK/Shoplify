﻿namespace Shoplify.Tests.ServicesTests
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
    }
}
