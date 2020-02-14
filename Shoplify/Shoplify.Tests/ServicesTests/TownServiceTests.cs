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
