using Shoplify.Domain;

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
    public class MessageServiceTests
    {
        private ShoplifyDbContext context;
        private IMessageService service;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ShoplifyDbContext>()
                .UseInMemoryDatabase(databaseName: "messages")
                .Options;

            this.context = new ShoplifyDbContext(options);

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            this.service = new MessageService(context);
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.DisposeAsync();
        }

        [Test]
        public async Task CreateMessageAsync_WithInvalidConversationId_ShouldThrowArgumentException()
        {
            var conversationId = "invalid";
            var receiverId = "rec";
            var senderId = "send";
            var text = "test";

            Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateMessageAsync(conversationId, senderId, receiverId, text));
        }

        [Test]
        public async Task CreateMessageAsync_WithInvalidSenderId_ShouldThrowArgumentException()
        {
            var receiverId = "rec";
            var senderId = "send";
            var text = "test";

            await context.Conversation.AddAsync(new Conversation
            {
                SellerId = receiverId,
                BuyerId = "firstUser",
                AdvertisementId = "test",
                StartedOn = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var conversation = await context.Conversation.FirstOrDefaultAsync();

            Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateMessageAsync(conversation.Id, senderId, receiverId, text));
        }

        [Test]
        public async Task CreateMessageAsync_WithInvalidReceiverId_ShouldThrowArgumentException()
        {
            var receiverId = "rec";
            var senderId = "send";
            var text = "test";

            await context.Conversation.AddAsync(new Conversation
            {
                SellerId = "second",
                BuyerId = senderId,
                AdvertisementId = "test",
                StartedOn = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var conversation = await context.Conversation.FirstOrDefaultAsync();

            Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateMessageAsync(conversation.Id, senderId, receiverId, text));
        }

        [Test]
        public async Task CreateMessageAsync_WithValidData_ShouldCreateSuccessfully()
        {
            var receiverId = "rec";
            var senderId = "send";
            var text = "test";

            await context.Conversation.AddAsync(new Conversation
            {
                SellerId = receiverId,
                BuyerId = senderId,
                AdvertisementId = "test",
                StartedOn = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var conversation = await context.Conversation.FirstOrDefaultAsync();

            var message = await service.CreateMessageAsync(conversation.Id, senderId, receiverId, text);

            var actualCount = context.Messages.Count();
            var expectedCount = 1;

            Assert.AreEqual(expectedCount, actualCount);
            Assert.IsNotNull(message.Id);
        }

        [Test]
        public async Task CreateMessageAsync_WithValidData_ShouldUpdateConversationStateSuccessfully()
        {
            var receiverId = "rec";
            var senderId = "send";
            var text = "test";

            await context.Conversation.AddAsync(new Conversation
            {
                SellerId = senderId,
                BuyerId = receiverId,
                AdvertisementId = "test",
                StartedOn = DateTime.UtcNow,
                IsReadByBuyer = true,
                IsArchivedByBuyer = true,
                IsReadBySeller = true,
                IsArchivedBySeller = true,
            });

            await context.SaveChangesAsync();

            var conversation = await context.Conversation.FirstOrDefaultAsync();

            await service.CreateMessageAsync(conversation.Id, senderId, receiverId, text);

            Assert.IsFalse(conversation.IsReadByBuyer);
            Assert.IsFalse(conversation.IsArchivedByBuyer);
            Assert.IsTrue(conversation.IsReadBySeller);
            Assert.IsTrue(conversation.IsArchivedBySeller);
        }

        [Test]
        public async Task GetAllByReceiverIdAsync_WithInvalidConversationId_ShouldThrowArgumentException()
        {
            var conversationId = "invalid";
            var receiverId = "rec";

            Assert.ThrowsAsync<ArgumentException>(async () => await service.GetAllByReceiverIdAsync(conversationId, receiverId));
        }

        [Test]
        public async Task GetAllByReceiverIdAsync_WithInvalidReceiverId_ShouldThrowArgumentException()
        {
            var receiverId = "invalid";

            await context.Conversation.AddAsync(new Conversation
            {
                SellerId = "second",
                BuyerId = "first",
                AdvertisementId = "test",
                StartedOn = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var conversation = await context.Conversation.FirstOrDefaultAsync();

            Assert.ThrowsAsync<ArgumentException>(async () => await service.GetAllByReceiverIdAsync(conversation.Id, receiverId));
        }

        [Test]
        public async Task GetAllByReceiverIdAsync_WithValidData_ShouldWorkCorrectly()
        {
            var text = "test";
            var firstUserId = "firstUser";
            var secondUserId = "secondUser";

            await context.Conversation.AddAsync(new Conversation
            {
                SellerId = firstUserId,
                BuyerId = secondUserId,
                AdvertisementId = "test",
                StartedOn = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var conversation = await context.Conversation.FirstOrDefaultAsync();

            await service.CreateMessageAsync(conversation.Id, secondUserId, firstUserId, text);
            await service.CreateMessageAsync(conversation.Id, firstUserId, secondUserId, text);

            var messages = await service.GetAllByReceiverIdAsync(conversation.Id, firstUserId);

            var actualCount = messages.Count();
            var expectedCount = 1;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllBySenderIdAsync_WithInvalidConversationId_ShouldThrowArgumentException()
        {
            var conversationId = "invalid";
            var senderId = "send";

            Assert.ThrowsAsync<ArgumentException>(async () => await service.GetAllBySenderIdAsync(conversationId, senderId));
        }

        [Test]
        public async Task GetAllBySenderIdAsync_WithInvalidSenderId_ShouldThrowArgumentException()
        {
            var senderId = "invalid";

            await context.Conversation.AddAsync(new Conversation
            {
                SellerId = "second",
                BuyerId = "first",
                AdvertisementId = "test",
                StartedOn = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var conversation = await context.Conversation.FirstOrDefaultAsync();

            Assert.ThrowsAsync<ArgumentException>(async () => await service.GetAllBySenderIdAsync(conversation.Id, senderId));
        }

        [Test]
        public async Task GetAllBySenderIdAsync_WithValidData_ShouldWorkCorrectly()
        {
            var text = "test";
            var firstUserId = "firstUserId";
            var secondUserId = "secondUserId";

            await context.Conversation.AddAsync(new Conversation
            {
                SellerId = firstUserId,
                BuyerId = secondUserId,
                AdvertisementId = "test",
                StartedOn = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var conversation = await context.Conversation.FirstOrDefaultAsync();

            await service.CreateMessageAsync(conversation.Id, firstUserId, secondUserId, text);
            await service.CreateMessageAsync(conversation.Id, secondUserId, firstUserId, text);

            var messages = await service.GetAllBySenderIdAsync(conversation.Id, firstUserId);

            var actualCount = messages.Count();
            var expectedCount = 1;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllInConversationAsync_WithNoMessages_ShouldReturnCorrectly()
        {
            var conversationId = "cId";

            var result = await service.GetAllInConversationAsync(conversationId);

            var actualCount = result.Count();
            var expectedCount = 0;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public async Task GetAllInConversationAsync_WithMessages_ShouldReturnCorrectly()
        {
            var text = "test";
            var firstUserId = "firstUserId";
            var secondUserId = "secondUserId";

            await context.Conversation.AddAsync(new Conversation
            {
                SellerId = firstUserId,
                BuyerId = secondUserId,
                AdvertisementId = "test",
                StartedOn = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var conversation = await context.Conversation.FirstOrDefaultAsync();

            await service.CreateMessageAsync(conversation.Id, firstUserId, secondUserId, text);
            await service.CreateMessageAsync(conversation.Id, secondUserId, firstUserId, text);

            var messages = await service.GetAllInConversationAsync(conversation.Id);

            var actualCount = messages.Count();
            var expectedCount = 2;

            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
