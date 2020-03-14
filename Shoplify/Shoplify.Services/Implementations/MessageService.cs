namespace Shoplify.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models.Message;
    using Shoplify.Web.Data;

    public class MessageService : IMessageService
    {
        private ShoplifyDbContext context;

        public MessageService(ShoplifyDbContext context)
        {
            this.context = context;
        }

        public Task<MessageServiceModel> CreateMessageAsync(string conversationId, string senderId, string receiverId, string text)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MessageServiceModel>> GetAllByReceiverIdAsync(string conversationId, string receiverId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MessageServiceModel>> GetAllBySenderIdAsync(string conversationId, string senderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MessageServiceModel>> GetAllInChatAsync(string conversationId)
        {
            throw new NotImplementedException();
        }
    }
}
