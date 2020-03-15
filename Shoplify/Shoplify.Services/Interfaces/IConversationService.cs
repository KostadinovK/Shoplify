namespace Shoplify.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Services.Models.Conversation;

    public interface IConversationService
    {
        Task<ConversationServiceModel> CreateConversationAsync(string buyerId, string sellerId, string adId);

        Task<bool> ConversationExistsAsync(string buyerId, string sellerId, string adId);

        Task<string> GetIdAsync(string buyerId, string sellerId, string adId);

        Task<bool> MarkConversationAsReadAsync(string conversationId, string userId);

        Task<int> GetAllUnReadByUserIdCountAsync(string userId);

        Task<IEnumerable<ConversationServiceModel>> GetAllByUserIdAsync(string userId);

        Task<bool> ArchiveAsync(string conversationId, string userId);

        Task<int> ArchiveAllAsync(string userId);
    }
}
