namespace Shoplify.Tests.ServicesTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using Shoplify.Services.Implementations;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.Data;

    [TestFixture]
    public class ConversationServiceTests
    {
        private ShoplifyDbContext context;
        private IConversationService service;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ShoplifyDbContext>()
                .UseInMemoryDatabase(databaseName: "conversation")
                .Options;

            this.context = new ShoplifyDbContext(options);

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            this.service = new ConversationService(context);
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.DisposeAsync();
        }

        [Test]
        public async Task CreateConversationAsync_WithValidData_ShouldCreateSuccessfully()
        {
            var firstUserId = "firstUser";
            var secondUserId = "secondUser";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(firstUserId, secondUserId, adId);

            var actualCount = context.Conversation.Count();
            var expectedCount = 1;

            Assert.AreEqual(expectedCount, actualCount);
            Assert.IsNotNull(conversation.Id);
        }

        [Test]
        public async Task ConversationExistsAsync_WithNotExistingConversation_ShouldReturnFalse()
        {
            Assert.IsFalse(await service.ConversationExistsAsync("firstUserId", "secondUserId", "adId"));
        }

        [Test]
        public async Task ConversationExistsAsync_WithExistingConversation_ShouldReturnTrue()
        {
            var firstUserId = "firstUser";
            var secondUserId = "secondUser";
            var adId = "ad";

            await service.CreateConversationAsync(firstUserId, secondUserId, adId);

            Assert.IsTrue(await service.ConversationExistsAsync(firstUserId, secondUserId, adId));
        }


        [Test]
        public async Task GetIdAsync_WithExistingConversation_ShouldReturnId()
        {
            var firstUserId = "firstUser";
            var secondUserId = "secondUser";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(firstUserId, secondUserId, adId);

            var id = await service.GetIdAsync(firstUserId, secondUserId, adId);

            Assert.AreEqual(conversation.Id, id);
        }

        [Test]
        public async Task MarkConversationAsReadAsync_WithInvalidConversationId_ShouldReturnFalse()
        {
            Assert.IsFalse(await service.MarkConversationAsReadAsync("invalid", "user"));
        }

        [Test]
        public async Task MarkConversationAsReadAsync_WithInvalidUserId_ShouldReturnFalse()
        {
            var firstUserId = "firstUser";
            var secondUserId = "secondUser";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(firstUserId, secondUserId, adId);

            Assert.IsFalse(await service.MarkConversationAsReadAsync(conversation.Id, "invalid"));
        }

        [Test]
        public async Task MarkConversationAsReadAsync_WhenFirstUserReadIt_ShouldWorkCorrectly()
        {
            var firstUserId = "firstUser";
            var secondUserId = "secondUser";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(firstUserId, secondUserId, adId);

            var result = await service.MarkConversationAsReadAsync(conversation.Id, firstUserId);

            var conversationFromDb = context.Conversation.SingleOrDefault(c => c.Id == conversation.Id);

            Assert.IsTrue(result);
            Assert.IsTrue(conversationFromDb.IsReadByFirstUser);
            Assert.IsFalse(conversationFromDb.IsReadBySecondUser);
        }

        [Test]
        public async Task MarkConversationAsReadAsync_WhenSecondUserReadIt_ShouldWorkCorrectly()
        {
            var firstUserId = "firstUser";
            var secondUserId = "secondUser";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(firstUserId, secondUserId, adId);

            var result = await service.MarkConversationAsReadAsync(conversation.Id, secondUserId);

            var conversationFromDb = context.Conversation.SingleOrDefault(c => c.Id == conversation.Id);

            Assert.IsTrue(result);
            Assert.IsTrue(conversationFromDb.IsReadBySecondUser);
            Assert.IsFalse(conversationFromDb.IsReadByFirstUser);
        }

        [Test]
        public async Task GetAllUnReadByUserIdCountAsync_WithNoConversations_ShouldReturnCorrectly()
        {
            var userId = "uId";

            var actualCount = await service.GetAllUnReadByUserIdCountAsync(userId);

            var expectedCount = 0;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllUnReadByUserIdCountAsync_WithConversations_ShouldReturnCorrectly()
        {
            var firstUserId = "firstUser";
            var secondUserId = "secondUser";
            var firstAdId = "ad1";
            var secondAdId = "ad2";
            var thirdAdId = "ad3";

            var firstConversation = await service.CreateConversationAsync(firstUserId, secondUserId, firstAdId);
            var secondConversation = await service.CreateConversationAsync(firstUserId, secondUserId, secondAdId);
            var thirdConversation = await service.CreateConversationAsync(firstUserId, secondUserId, thirdAdId);

            await service.MarkConversationAsReadAsync(secondConversation.Id, firstUserId);
            await service.MarkConversationAsReadAsync(thirdConversation.Id, secondUserId);

            var actualCount = await service.GetAllUnReadByUserIdCountAsync(firstUserId);

            var expectedCount = 2;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllByUserIdAsync_WithNoConversations_ShouldReturnCorrectly()
        {
            var userId = "test";

            var result = await service.GetAllByUserIdAsync(userId);

            var actualCount = result.Count();

            var expectedCount = 0;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllByUserIdAsync_WithConversations_ShouldReturnCorrectly()
        {
            var firstUserId = "firstUser";
            var secondUserId = "secondUser";
            var firstAdId = "ad1";
            var secondAdId = "ad2";
            var thirdAdId = "ad3";

            var firstConversation = await service.CreateConversationAsync(firstUserId, secondUserId, firstAdId);
            var secondConversation = await service.CreateConversationAsync(firstUserId, secondUserId, secondAdId);
            var thirdConversation = await service.CreateConversationAsync(firstUserId, secondUserId, thirdAdId);

            await service.ArchiveAsync(thirdConversation.Id, firstUserId);

            var result = await service.GetAllByUserIdAsync(firstUserId);

            var actualCount = result.Count();
            var expectedCount = 2;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task ArchiveAsync_WithInvalidConversationId_ShouldReturnFalse()
        {
            Assert.IsFalse(await service.ArchiveAsync("invalid", "user"));
        }

        [Test]
        public async Task ArchiveAsync_WithInvalidUserId_ShouldReturnFalse()
        {
            var firstUserId = "firstUser";
            var secondUserId = "secondUser";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(firstUserId, secondUserId, adId);

            Assert.IsFalse(await service.ArchiveAsync(conversation.Id, "invalid"));
        }

        [Test]
        public async Task ArchiveAsync_WhenFirstUserArchiveIt_ShouldWorkCorrectly()
        {
            var firstUserId = "firstUser";
            var secondUserId = "secondUser";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(firstUserId, secondUserId, adId);

            var result = await service.ArchiveAsync(conversation.Id, firstUserId);

            var conversationFromDb = context.Conversation.SingleOrDefault(c => c.Id == conversation.Id);

            Assert.IsTrue(result);
            Assert.IsTrue(conversationFromDb.IsArchivedByFirstUser);
            Assert.IsFalse(conversationFromDb.IsArchivedBySecondUser);
        }

        [Test]
        public async Task ArchiveAsync_WhenSecondUserArciveIt_ShouldWorkCorrectly()
        {
            var firstUserId = "firstUser";
            var secondUserId = "secondUser";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(firstUserId, secondUserId, adId);

            var result = await service.ArchiveAsync(conversation.Id, secondUserId);

            var conversationFromDb = context.Conversation.SingleOrDefault(c => c.Id == conversation.Id);

            Assert.IsTrue(result);
            Assert.IsTrue(conversationFromDb.IsArchivedBySecondUser);
            Assert.IsFalse(conversationFromDb.IsArchivedByFirstUser);
        }
    }
}
