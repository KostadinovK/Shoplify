using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Moq;
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

            var store = new Mock<IUserStore<User>>();
            var mgr = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<User>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<User>());

            List<User> ls = new List<User>();

            mgr.Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<User, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

            this.service = new CommentService(context, mgr.Object);
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
