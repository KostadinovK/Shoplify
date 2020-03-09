namespace Shoplify.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Services.Models.Notification;

    public interface INotificationService
    {
        Task<NotificationServiceModel> CreateNotificationAsync(string text, string actionLink);

        Task<bool> MarkNotificationAsReadAsync(string notificationId, string userId);

        Task<int> MarkAllNotificationsAsReadAsync(string userId);

        Task<int> GetAllUnReadByUserIdCountAsync(string userId);

        Task<IEnumerable<NotificationServiceModel>> GetAllUnReadByUserIdAsync(string userId);

        Task<bool> AssignNotificationToUserAsync(string notificationId, string userId);

        Task<int> AssignNotificationToUsersAsync(string notificationId, List<string> userIds);
    }
}
