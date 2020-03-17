namespace Shoplify.Tests.ServicesTests
{
    using System.Linq;
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
            var buyerId = "firstUser";
            var sellerId = "secondUser";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(buyerId, sellerId, adId);

            var actualCount = context.Conversation.Count();
            var expectedCount = 1;

            Assert.AreEqual(expectedCount, actualCount);
            Assert.IsNotNull(conversation.Id);
        }

        [Test]
        public async Task ConversationExistsAsync_WithNotExistingConversation_ShouldReturnFalse()
        {
            Assert.IsFalse(await service.ConversationExistsAsync("buyerId", "sellerId", "adId"));
        }

        [Test]
        public async Task ConversationExistsAsync_WithExistingConversation_ShouldReturnTrue()
        {
            var buyerId = "firstUser";
            var sellerId = "secondUser";
            var adId = "ad";

            await service.CreateConversationAsync(buyerId, sellerId, adId);

            Assert.IsTrue(await service.ConversationExistsAsync(buyerId, sellerId, adId));
        }


        [Test]
        public async Task GetIdAsync_WithExistingConversation_ShouldReturnId()
        {
            var buyerId = "firstUser";
            var sellerId = "secondUser";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(buyerId, sellerId, adId);

            var id = await service.GetIdAsync(buyerId, sellerId, adId);

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
            var buyerId = "firstUser";
            var sellerId = "secondUser";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(buyerId, sellerId, adId);

            Assert.IsFalse(await service.MarkConversationAsReadAsync(conversation.Id, "invalid"));
        }

        [Test]
        public async Task MarkConversationAsReadAsync_WhenBuyerReadIt_ShouldWorkCorrectly()
        {
            var buyerId = "firstUser";
            var sellerId = "secondUser";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(buyerId, sellerId, adId);

            var result = await service.MarkConversationAsReadAsync(conversation.Id, buyerId);

            var conversationFromDb = context.Conversation.SingleOrDefault(c => c.Id == conversation.Id);

            Assert.IsTrue(result);
            Assert.IsTrue(conversationFromDb.IsReadByBuyer);
            Assert.IsTrue(conversationFromDb.IsReadBySeller);
        }

        [Test]
        public async Task MarkConversationAsReadAsync_WhenSellerReadIt_ShouldWorkCorrectly()
        {
            var buyerId = "firstUser";
            var sellerId = "secondUser";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(buyerId, sellerId, adId);

            var result = await service.MarkConversationAsReadAsync(conversation.Id, sellerId);

            var conversationFromDb = context.Conversation.SingleOrDefault(c => c.Id == conversation.Id);

            Assert.IsTrue(result);
            Assert.IsTrue(conversationFromDb.IsReadBySeller);
            Assert.IsTrue(conversationFromDb.IsReadByBuyer);
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
            var buyerId = "firstUser";
            var sellerId = "secondUser";
            var firstAdId = "ad1";
            var secondAdId = "ad2";
            var thirdAdId = "ad3";

            var firstConversation = await service.CreateConversationAsync(buyerId, sellerId, firstAdId);
            var secondConversation = await service.CreateConversationAsync(buyerId, sellerId, secondAdId);
            var thirdConversation = await service.CreateConversationAsync(buyerId, sellerId, thirdAdId);

            await service.MarkConversationAsReadAsync(secondConversation.Id, buyerId);
            await service.MarkConversationAsReadAsync(thirdConversation.Id, sellerId);

            var actualCount = await service.GetAllUnReadByUserIdCountAsync(buyerId);

            var expectedCount = 0;

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
            var buyerId = "firstUser";
            var sellerId = "secondUser";
            var firstAdId = "ad1";
            var secondAdId = "ad2";
            var thirdAdId = "ad3";

            var firstConversation = await service.CreateConversationAsync(buyerId, sellerId, firstAdId);
            var secondConversation = await service.CreateConversationAsync(buyerId, sellerId, secondAdId);
            var thirdConversation = await service.CreateConversationAsync(buyerId, sellerId, thirdAdId);

            await service.ArchiveAsync(thirdConversation.Id, buyerId);

            var result = await service.GetAllByUserIdAsync(buyerId);

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
            var buyerId = "firstUser";
            var sellerId = "secondUser";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(buyerId, sellerId, adId);

            Assert.IsFalse(await service.ArchiveAsync(conversation.Id, "invalid"));
        }

        [Test]
        public async Task ArchiveAsync_WhenBuyerArchiveIt_ShouldWorkCorrectly()
        {
            var buyerId = "firstUser";
            var sellerId = "secondUser";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(buyerId, sellerId, adId);

            var result = await service.ArchiveAsync(conversation.Id, buyerId);

            var conversationFromDb = context.Conversation.SingleOrDefault(c => c.Id == conversation.Id);

            Assert.IsTrue(result);
            Assert.IsTrue(conversationFromDb.IsArchivedByBuyer);
            Assert.IsFalse(conversationFromDb.IsArchivedBySeller);
        }

        [Test]
        public async Task ArchiveAsync_WhenSellerArchiveIt_ShouldWorkCorrectly()
        {
            var buyerId = "firstUser";
            var sellerId = "secondUser";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(buyerId, sellerId, adId);

            var result = await service.ArchiveAsync(conversation.Id, sellerId);

            var conversationFromDb = context.Conversation.SingleOrDefault(c => c.Id == conversation.Id);

            Assert.IsTrue(result);
            Assert.IsTrue(conversationFromDb.IsArchivedBySeller);
            Assert.IsFalse(conversationFromDb.IsArchivedByBuyer);
        }

        [Test]
        public async Task ArchiveAllAsync_WithNoConversations_ShouldReturnCorrectly()
        {
            var userId = "test";

            var actualCount = await service.ArchiveAllAsync(userId);

            var expectedCount = 0;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task ArchiveAllAsync_WithConversations_ShouldReturnCorrectly()
        {
            var userId = "user";
            var secondUserId = "secondUser";
            var adId = "ad";
            var secondAdId = "ad2";

            var firstConversation = await service.CreateConversationAsync(userId, secondUserId, adId);
            var secondConversation = await service.CreateConversationAsync(userId, secondUserId, secondAdId);

            var actualCount = await service.ArchiveAllAsync(userId);
            var expectedCount = 2;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetByIdAsync_ShouldGetCorrectly()
        {
            var buyerId = "user";
            var sellerId = "seller";
            var adId = "ad";

            var conversation = await service.CreateConversationAsync(buyerId, sellerId, adId);

            var gettedConversation = await service.GetByIdAsync(conversation.Id);

            AssertEx.PropertyValuesAreEquals(conversation, gettedConversation);
        }
    }
}
