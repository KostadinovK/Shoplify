namespace Shoplify.Tests.ServicesTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using Shoplify.Services.Implementations;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.Data;

    [TestFixture]
    public class NotificationServiceTests
    {
        private ShoplifyDbContext context;
        private INotificationService service;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ShoplifyDbContext>()
                .UseInMemoryDatabase(databaseName: "notifications")
                .Options;

            this.context = new ShoplifyDbContext(options);

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            this.service = new NotificationService(context);
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.DisposeAsync();
        }

        [Test]
        public async Task CreateNotificationAsync_WithValidData_ShouldCreateSuccessfully()
        {
            var text = "Test Notification";
            var actionLink = "/Link/Test";

            var notification = await service.CreateNotificationAsync(text, actionLink);

            var actualCount = context.Notifications.Count();
            var expectedCount = 1;

            Assert.AreEqual(expectedCount, actualCount);
            Assert.IsNotNull(notification.Id);
        }

        [Test]
        public async Task MarkNotificationAsReadAsync_WithInValidNotificationId_ShouldReturnFalse()
        {
            Assert.IsFalse(await service.MarkNotificationAsReadAsync("notification", "user"));
        }

        [Test]
        public async Task MarkNotificationAsReadAsync_WithValidNotificationId_ShouldReturnTrue()
        {
            var notificationId = "nId";
            var userId = "uId";

            await service.AssignNotificationToUserAsync(notificationId, userId);

            var result = await service.MarkNotificationAsReadAsync(notificationId, userId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task MarkAllNotificationsAsReadAsync_WithNoNotificationsUserId_ShouldReturnCorrectly()
        {
            var userId = "uId";

            var actualMarkedNotificationsCount = await service.MarkAllNotificationsAsReadAsync(userId);

            var expectedMarkedNotificationsCount = 0;

            Assert.AreEqual(expectedMarkedNotificationsCount, actualMarkedNotificationsCount);
        }

        [Test]
        public async Task MarkAllNotificationsAsReadAsync_WithNotificationsForUserId_ShouldReturnCorrectly()
        {
            var userId = "uId";

            await service.AssignNotificationToUserAsync("n1", userId);
            await service.AssignNotificationToUserAsync("n2", userId);
            await service.AssignNotificationToUserAsync("n3", userId);

            var actualMarkedNotificationsCount = await service.MarkAllNotificationsAsReadAsync(userId);

            var expectedMarkedNotificationsCount = 3;

            Assert.AreEqual(expectedMarkedNotificationsCount, actualMarkedNotificationsCount);
        }

        [Test]
        public async Task GetAllUnReadByUserIdCountAsync_WithNoNotificationsUserId_ShouldReturnCorrectly()
        {
            var userId = "uId";

            var actualMarkedNotificationsCount = await service.GetAllUnReadByUserIdCountAsync(userId);

            var expectedMarkedNotificationsCount = 0;

            Assert.AreEqual(expectedMarkedNotificationsCount, actualMarkedNotificationsCount);
        }

        [Test]
        public async Task GetAllUnReadByUserIdCountAsync_WithNotificationsUserId_ShouldReturnCorrectly()
        {
            var userId = "uId";

            await service.AssignNotificationToUserAsync("n1", userId);
            await service.AssignNotificationToUserAsync("n2", userId);
            await service.AssignNotificationToUserAsync("n3", userId);

            await service.MarkNotificationAsReadAsync("n1", userId);

            var actualMarkedNotificationsCount = await service.GetAllUnReadByUserIdCountAsync(userId);

            var expectedMarkedNotificationsCount = 2;

            Assert.AreEqual(expectedMarkedNotificationsCount, actualMarkedNotificationsCount);
        }

        [Test]
        public async Task GetAllUnReadByUserIdAsync_WithNoNotificationsUserId_ShouldReturnCorrectly()
        {
            var userId = "uId";

            var notifications = await service.GetAllUnReadByUserIdAsync(userId);

            var actualMarkedNotificationsCount = notifications.Count();

            var expectedMarkedNotificationsCount = 0;

            Assert.AreEqual(expectedMarkedNotificationsCount, actualMarkedNotificationsCount);
        }

        [Test]
        public async Task GetAllUnReadByUserIdAsync_WithNotificationsUserId_ShouldReturnCorrectly()
        {
            var userId = "uId";

            var firstNotification = await service.CreateNotificationAsync("test", "test");
            var secondNotification = await service.CreateNotificationAsync("test", "test");
            var thirdNotification = await service.CreateNotificationAsync("test", "test");

            await service.AssignNotificationToUserAsync(firstNotification.Id.ToString(), userId);
            await service.AssignNotificationToUserAsync(secondNotification.Id.ToString(), userId);
            await service.AssignNotificationToUserAsync(thirdNotification.Id.ToString(), userId);

            await service.MarkNotificationAsReadAsync(firstNotification.Id.ToString(), userId);

            var notifications = await service.GetAllUnReadByUserIdAsync(userId);

            var actualMarkedNotificationsCount = notifications.Count();
            var expectedMarkedNotificationsCount = 2;

            Assert.AreEqual(expectedMarkedNotificationsCount, actualMarkedNotificationsCount);
        }

        [Test]
        public async Task AssignNotificationToUserAsync_WithNotAlreadyAssignedNotification_ShouldAssignSuccessfully()
        {
            var notificationId = "nId";
            var userId = "uId";

            var result = await service.AssignNotificationToUserAsync(notificationId, userId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task AssignNotificationToUserAsync_WithAlreadyAssignedNotification_ShouldReturnFalse()
        {
            var notificationId = "nId";
            var userId = "uId";

            await service.AssignNotificationToUserAsync(notificationId, userId);

            var result = await service.AssignNotificationToUserAsync(notificationId, userId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AssignNotificationToUsersAsync_WithNotAlreadyAssignedNotification_ShouldAssignSuccessfully()
        {
            var notificationId = "nId";

            var userIds = new List<string>(){ "u1", "u2", "u3"};

            await service.AssignNotificationToUserAsync(notificationId, userIds[1]);

            var actualCount = await service.AssignNotificationToUsersAsync(notificationId, userIds);
            var expectedCount = 2;

            Assert.AreEqual(expectedCount, actualCount);
        }

    }
}
