using System.Linq;
using Shoplify.Domain;
using Shoplify.Web.Data;

namespace Shoplify.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models.Message;

    public class MessageService : IMessageService
    {
        private ShoplifyDbContext context;

        public MessageService(ShoplifyDbContext context)
        {
            this.context = context;
        }

        public async Task<MessageServiceModel> CreateMessageAsync(string senderId, string receiverId, string adId, string text)
        {
            throw new NotImplementedException();
        }

        public Task<int> MarkMessagesAsReadAsync(string adId, string receiverId, string senderId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAllUnReadByUserIdCountAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MessageServiceModel>> GetAllBySenderIdAsync(string senderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MessageServiceModel>> GetAllByReceiverIdAsync(string receiverId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MessageServiceModel>> GetAllInChatAsync(string senderId, string receiverId, string adId)
        {
            throw new NotImplementedException();
        }
    }
}
