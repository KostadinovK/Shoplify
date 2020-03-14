namespace Shoplify.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models.Message;
    using Shoplify.Web.Data;

    public class MessageService : IMessageService
    {
        private const string InvalidConversationId = "Conversation with the provided id doesnt exist!";
        private const string InvalidSenderId = "Sender with the provided sender id doesnt exist in this conversation!";
        private const string InvalidReceiverId = "Receiver with the provided receiver id doesnt exist in this conversation!";

        private ShoplifyDbContext context;

        public MessageService(ShoplifyDbContext context)
        {
            this.context = context;
        }

        public async Task<MessageServiceModel> CreateMessageAsync(string conversationId, string senderId, string receiverId, string text)
        {
            var conversation = context.Conversation.SingleOrDefault(c => c.Id == conversationId);

            if (conversation == null)
            {
                throw new ArgumentException(InvalidConversationId);
            }

            if (conversation.FirstUserId != senderId && conversation.SecondUserId != senderId)
            {
                throw new ArgumentException(InvalidSenderId);
            }

            if (conversation.FirstUserId != receiverId && conversation.SecondUserId != receiverId)
            {
                throw new ArgumentException(InvalidReceiverId);
            }

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                ConversationId = conversationId,
                SendOn = DateTime.UtcNow,
                Text = text
            };

            await context.AddAsync(message);

            if (conversation.FirstUserId == receiverId && conversation.IsReadByFirstUser)
            {
                conversation.IsReadByFirstUser = false;
                conversation.IsArchivedByFirstUser = false;
                context.Conversation.Update(conversation);
            }
            else if (conversation.SecondUserId == receiverId && conversation.IsReadBySecondUser)
            {
                conversation.IsReadBySecondUser = false;
                conversation.IsArchivedBySecondUser = false;
                context.Conversation.Update(conversation);
            }

            await context.SaveChangesAsync();

            return new MessageServiceModel
            {
                Id = message.Id,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                ConversationId = message.ConversationId,
                SendOn = message.SendOn,
                Text = message.Text
            };
        }

        public async Task<IEnumerable<MessageServiceModel>> GetAllByReceiverIdAsync(string conversationId, string receiverId)
        {
            var conversation = context.Conversation.SingleOrDefault(c => c.Id == conversationId);

            if (conversation == null)
            {
                throw new ArgumentException(InvalidConversationId);
            }

            if (conversation.FirstUserId != receiverId && conversation.SecondUserId != receiverId)
            {
                throw new ArgumentException(InvalidReceiverId);
            }

            return await context.Messages
                .Where(m => m.ConversationId == conversationId && m.ReceiverId == receiverId)
                .Select(m => new MessageServiceModel
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    ConversationId = m.ConversationId,
                    SendOn = m.SendOn,
                    Text = m.Text
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MessageServiceModel>> GetAllBySenderIdAsync(string conversationId, string senderId)
        {
            var conversation = context.Conversation.SingleOrDefault(c => c.Id == conversationId);

            if (conversation == null)
            {
                throw new ArgumentException(InvalidConversationId);
            }

            if (conversation.FirstUserId != senderId && conversation.SecondUserId != senderId)
            {
                throw new ArgumentException(InvalidSenderId);
            }

            return await context.Messages
                .Where(m => m.ConversationId == conversationId && m.SenderId == senderId)
                .Select(m => new MessageServiceModel
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    ConversationId = m.ConversationId,
                    SendOn = m.SendOn,
                    Text = m.Text
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MessageServiceModel>> GetAllInChatAsync(string conversationId)
        {
            return await context.Messages
                .Where(m => m.ConversationId == conversationId)
                .Select(m => new MessageServiceModel
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    ConversationId = m.ConversationId,
                    SendOn = m.SendOn,
                    Text = m.Text
                })
                .ToListAsync();
        }
    }
}
