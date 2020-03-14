namespace Shoplify.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models.Conversation;

    public class ConversationService : IConversationService
    {
        public Task<ConversationServiceModel> CreateConversationAsync(string firstUserId, string secondUserId, string adId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MarkConversationAsReadAsync(string conversationId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAllUnReadByUserIdCountAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConversationServiceModel>> GetAllByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
