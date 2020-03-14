namespace Shoplify.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Services.Models.Message;

    public interface IMessageService
    {
        Task<MessageServiceModel> CreateMessageAsync(string senderId, string receiverId, string adId, string text);

        Task<int> MarkMessagesAsReadAsync(string adId, string receiverId, string senderId);

        Task<int> GetAllUnReadByUserIdCountAsync(string userId);

        Task<IEnumerable<MessageServiceModel>> GetAllBySenderIdAsync(string senderId);

        Task<IEnumerable<MessageServiceModel>> GetAllByReceiverIdAsync(string receiverId);

        Task<IEnumerable<MessageServiceModel>> GetAllInChatAsync(string senderId, string receiverId, string adId);
    }
}
