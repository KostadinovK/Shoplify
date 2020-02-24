using System;
using System.Linq;
using Shoplify.Domain;

namespace Shoplify.Tests.ServicesTests
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using Shoplify.Services.Implementations;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.Data;

    [TestFixture]
    public class CommentServiceTests
    {
        private ShoplifyDbContext context;
        private ICommentService service;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ShoplifyDbContext>()
                .UseInMemoryDatabase(databaseName: "comments")
                .Options;

            this.context = new ShoplifyDbContext(options);

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            this.service = new CommentService(context);
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.DisposeAsync();
        }

        [Test]
        public async Task GetAllByAdIdAsync_GetWithoutComments_ShouldReturnEmptyCollection()
        {
            var comments = await service.GetAllByAdIdAsync("test");

            var expectedCount = 0;
            var actualCount = comments.Count();
            
            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllByAdIdAsync_GetWithComments_ShouldReturnCorrectly()
        {
            var comment = new Comment
            {
                AdvertisementId = "test",
                Text = "test message",
                WrittenOn = DateTime.UtcNow,
                UserId = "test user"
            };

            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();

            var comments = await service.GetAllByAdIdAsync("test");

            var expectedCount = 1;
            var actualCount = comments.Count();

            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
