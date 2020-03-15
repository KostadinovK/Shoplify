namespace Shoplify.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Services.Models.Message;

    public interface IMessageService
    {
        Task<MessageServiceModel> CreateMessageAsync(string conversationId, string senderId, string receiverId, string text);

        Task<IEnumerable<MessageServiceModel>> GetAllBySenderIdAsync(string conversationId, string senderId);

        Task<IEnumerable<MessageServiceModel>> GetAllByReceiverIdAsync(string conversationId, string receiverId);

        Task<IEnumerable<MessageServiceModel>> GetAllInConversationAsync(string conversationId);
    }
}
