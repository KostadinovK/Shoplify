using System;
using System.Linq;

namespace Shoplify.Tests.ServicesTests
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using Shoplify.Domain;
    using Shoplify.Services.Implementations;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models;
    using Shoplify.Web.Data;

    [TestFixture]
    public class TownServiceTests
    {
        private ShoplifyDbContext context;
        private ITownService service;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ShoplifyDbContext>()
                .UseInMemoryDatabase(databaseName: "towns")
                .Options;

            this.context = new ShoplifyDbContext(options);

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            this.service = new TownService(context);
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.DisposeAsync();
        }

        [Test]
        public async Task GetByIdAsync_WithInvalidId_ShouldThrowArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await service.GetByIdAsync("invalidId"));
        }

        [Test]
        public async Task GetByIdAsync_WithValidId_ShouldReturnCorrectly()
        {
            var townName = "test";

            var town = new Town
            {
                Name = townName
            };

            await context.Towns.AddAsync(town);
            await context.SaveChangesAsync();

            var townFromDb = await context.Towns.FirstOrDefaultAsync(t => t.Name == townName);

            var actualTown = await service.GetByIdAsync(townFromDb.Id.ToString());

            var expectedTown = new TownServiceModel()
            {
                Name = townFromDb.Name,
                Id = townFromDb.Id,
            };

            AssertEx.PropertyValuesAreEquals(actualTown, expectedTown);
        }

        [Test]
        public async Task GetByNameAsync_WithInvalidName_ShouldThrowArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await service.GetByNameAsync("invalidName"));
        }

        [Test]
        public async Task GetByNameAsync_WithValidName_ShouldReturnCorrectly()
        {
            var townName = "test";

            var town = new Town()
            {
                Name = townName
            };

            await context.Towns.AddAsync(town);
            await context.SaveChangesAsync();

            var actualTown = await service.GetByNameAsync(town.Name);

            var expectedTown = new TownServiceModel()
            {
                Name = town.Name,
                Id = town.Id,
            };

            AssertEx.PropertyValuesAreEquals(actualTown, expectedTown);
        }


        [Test]
        public async Task GetAll_ShouldReturnCorrectly()
        {
            var townName = "test";
            var townName2 = "test2";

            var town = new Town
            {
                Name = townName
            };

            var town2 = new Town
            {
                Name = townName2
            };

            await context.Towns.AddAsync(town);
            await context.Towns.AddAsync(town2);

            await context.SaveChangesAsync();

            var towns = service.GetAll().ToList();

            var expectedTown = new TownServiceModel()
            {
                Name = town.Name,
                Id = town.Id,
            };

            var expectedTown2 = new TownServiceModel()
            {
                Name = town2.Name,
                Id = town2.Id,
            };

            var expectedCategoriesCount = 2;
            var actualCategoriesCount = towns.Count;

            Assert.AreEqual(expectedCategoriesCount, actualCategoriesCount);
            AssertEx.PropertyValuesAreEquals(towns[0], expectedTown);
            AssertEx.PropertyValuesAreEquals(towns[1], expectedTown2);
        }
    }
}
