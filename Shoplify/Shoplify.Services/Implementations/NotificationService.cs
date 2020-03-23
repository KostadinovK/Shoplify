namespace Shoplify.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models.Notification;
    using Shoplify.Web.Data;

    public class NotificationService : INotificationService
    {
        private ShoplifyDbContext context;

        public NotificationService(ShoplifyDbContext context)
        {
            this.context = context;
        }

        public async Task<NotificationServiceModel> CreateNotificationAsync(string text, string actionLink)
        {
            var notification = new Notification 
            {
                Text = text,
                ActionLink = actionLink,
                CreatedOn = DateTime.UtcNow
            };

            await context.Notifications.AddAsync(notification);
            await context.SaveChangesAsync();

            return new NotificationServiceModel
            {
                Text = notification.Text,
                ActionLink = notification.ActionLink,
                CreatedOn = notification.CreatedOn,
                Id = notification.Id
            };
        }

        public async Task<bool> MarkNotificationAsReadAsync(string notificationId, string userId)
        {
            var notification =
                await context.UsersNotifications.SingleOrDefaultAsync(un =>
                    un.UserId == userId && un.NotificationId == notificationId);

            if (notification == null)
            {
                return false;
            }

            notification.IsRead = true;

            context.UsersNotifications.Update(notification);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<int> MarkAllNotificationsAsReadAsync(string userId)
        {
            var notifications =
                await context.UsersNotifications.Where(un =>
                    un.UserId == userId && un.IsRead == false)
                    .ToListAsync();

            foreach (var notification in notifications)
            {
                await MarkNotificationAsReadAsync(notification.NotificationId, notification.UserId);
            }

            return notifications.Count;
        }

        public async Task<int> GetAllUnReadByUserIdCountAsync(string userId)
        {
            var notifications =
                await context.UsersNotifications.Where(un =>
                    un.UserId == userId && un.IsRead == false)
                    .ToListAsync();

            return notifications.Count;
        }

        public async Task<IEnumerable<NotificationServiceModel>> GetAllUnReadByUserIdAsync(string userId)
        {
            var usersNotifications =
                await context.UsersNotifications.Where(un =>
                        un.UserId == userId && un.IsRead == false)
                    .OrderByDescending(n => n.Notification.CreatedOn)
                    .ToListAsync();

            var result = new List<NotificationServiceModel>();

            foreach (var userNotification in usersNotifications)
            {
                var notification = await context.Notifications.SingleOrDefaultAsync(n => n.Id == userNotification.NotificationId);

                result.Add(new NotificationServiceModel()
                {
                    ActionLink = notification.ActionLink,
                    CreatedOn = notification.CreatedOn,
                    Id = notification.Id,
                    Text = notification.Text
                });
            }

            return result;
        }

        public async Task<bool> AssignNotificationToUserAsync(string notificationId, string userId)
        {
            var userNotification = new UserNotification
            {
                IsRead = false,
                NotificationId = notificationId,
                UserId = userId
            };

            if (await context.UsersNotifications.AnyAsync(un => un.NotificationId == notificationId && un.UserId == userId))
            {
                return false;
            }

            await context.UsersNotifications.AddAsync(userNotification);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<int> AssignNotificationToUsersAsync(string notificationId, List<string> userIds)
        {
            var count = 0;

            foreach (var userId in userIds)
            {
                if (await AssignNotificationToUserAsync(notificationId, userId))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
