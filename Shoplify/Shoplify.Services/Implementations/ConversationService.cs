namespace Shoplify.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models.Conversation;
    using Shoplify.Web.Data;

    public class ConversationService : IConversationService
    {
        private ShoplifyDbContext context;

        public ConversationService(ShoplifyDbContext context)
        {
            this.context = context;
        }

        public async Task<ConversationServiceModel> CreateConversationAsync(string firstUserId, string secondUserId, string adId)
        {
            var conversation = new Conversation
            {
                FirstUserId = firstUserId,
                SecondUserId = secondUserId,
                AdvertisementId = adId,
                IsReadByFirstUser = false,
                IsReadBySecondUser = false,
                StartedOn = DateTime.UtcNow
            };

            await context.AddAsync(conversation);
            await context.SaveChangesAsync();

            return new ConversationServiceModel
            {
                Id = conversation.Id,
                FirstUserId = conversation.FirstUserId,
                SecondUserId = conversation.SecondUserId,
                AdvertisementId = conversation.AdvertisementId,
                IsReadByFirstUser = conversation.IsReadByFirstUser,
                IsReadBySecondUser = conversation.IsReadBySecondUser,
                StartedOn = conversation.StartedOn
            };
        }

        public async Task<bool> ConversationExistsAsync(string firstUserId, string secondUserId, string adId)
        {
            return await context.Conversation
                .AnyAsync(c => c.AdvertisementId == adId && 
                               (c.FirstUserId == firstUserId || c.SecondUserId == firstUserId) &&
                               (c.FirstUserId == secondUserId || c.SecondUserId == secondUserId));
        }

        public async Task<bool> MarkConversationAsReadAsync(string conversationId, string userId)
        {
            if (!context.Conversation.Any(c => c.Id == conversationId))
            {
                return false;
            }

            var conversation = context.Conversation.SingleOrDefault(c => c.Id == conversationId);

            if (conversation.FirstUserId == userId)
            {
                conversation.IsReadByFirstUser = true;
            }
            else if (conversation.SecondUserId == userId)
            {
                conversation.IsReadBySecondUser = true;
            }else
            {
                return false;
            }

            context.Conversation.Update(conversation);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetAllUnReadByUserIdCountAsync(string userId)
        {
            var conversations = await context.Conversation.Where(c =>
                (c.FirstUserId == userId && !c.IsReadByFirstUser) ||
                (c.SecondUserId == userId && !c.IsReadBySecondUser)).ToListAsync();

            return conversations.Count;
        }

        public async Task<IEnumerable<ConversationServiceModel>> GetAllByUserIdAsync(string userId)
        {
           return await context.Conversation.Where(c =>
                c.FirstUserId == userId ||
                c.SecondUserId == userId)
                .Select(c => new ConversationServiceModel
                {
                    Id = c.Id,
                    FirstUserId = c.FirstUserId,
                    SecondUserId = c.SecondUserId,
                    AdvertisementId = c.AdvertisementId,
                    IsReadByFirstUser = c.IsReadByFirstUser,
                    IsReadBySecondUser = c.IsReadBySecondUser,
                    StartedOn = c.StartedOn
                })
                .ToListAsync();
        }
    }
}
