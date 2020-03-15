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

        public async Task<ConversationServiceModel> CreateConversationAsync(string buyerId, string sellerId, string adId)
        {
            var conversation = new Conversation
            {
                BuyerId = buyerId,
                SellerId = sellerId,
                AdvertisementId = adId,
                IsReadByBuyer = false,
                IsReadBySeller = false,
                IsArchivedByBuyer = false,
                IsArchivedBySeller = false,
                StartedOn = DateTime.UtcNow
            };

            await context.AddAsync(conversation);
            await context.SaveChangesAsync();

            return new ConversationServiceModel
            {
                Id = conversation.Id,
                BuyerId = conversation.BuyerId,
                SellerId = conversation.SellerId,
                AdvertisementId = conversation.AdvertisementId,
                IsReadByBuyer = conversation.IsReadByBuyer,
                IsReadBySeller = conversation.IsReadBySeller,
                IsArchivedByBuyer = conversation.IsArchivedByBuyer,
                IsArchivedBySeller = conversation.IsArchivedBySeller,
                StartedOn = conversation.StartedOn
            };
        }

        public async Task<bool> ConversationExistsAsync(string buyerId, string sellerId, string adId)
        {
            return await context.Conversation
                .AnyAsync(c => c.AdvertisementId == adId && 
                               (c.BuyerId == buyerId || c.SellerId == buyerId) &&
                               (c.BuyerId == sellerId || c.SellerId == sellerId));
        }

        public async Task<string> GetIdAsync(string buyerId, string sellerId, string adId)
        {
            if (!await ConversationExistsAsync(buyerId, sellerId, adId))
            {
                return null;
            }

            return context.Conversation.SingleOrDefault(c =>
                c.AdvertisementId == adId && (c.BuyerId == buyerId || c.SellerId == buyerId) &&
                (c.SellerId == buyerId || c.SellerId == sellerId)).Id;
        }

        public async Task<bool> MarkConversationAsReadAsync(string conversationId, string userId)
        {
            if (!await context.Conversation.AnyAsync(c => c.Id == conversationId))
            {
                return false;
            }

            var conversation = await context.Conversation.SingleOrDefaultAsync(c => c.Id == conversationId);

            if (conversation.BuyerId == userId)
            {
                conversation.IsReadByBuyer = true;
            }
            else if (conversation.SellerId == userId)
            {
                conversation.IsReadBySeller = true;
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
                (c.BuyerId == userId && !c.IsReadByBuyer && !c.IsArchivedByBuyer) ||
                (c.SellerId == userId && !c.IsReadBySeller && !c.IsArchivedBySeller)).ToListAsync();

            return conversations.Count;
        }

        public async Task<IEnumerable<ConversationServiceModel>> GetAllByUserIdAsync(string userId)
        {
           return await context.Conversation.Where(c =>
                (c.BuyerId == userId && !c.IsArchivedByBuyer) ||
                (c.SellerId == userId && !c.IsArchivedBySeller))
                .Select(c => new ConversationServiceModel
                {
                    Id = c.Id,
                    BuyerId = c.BuyerId,
                    SellerId = c.SellerId,
                    AdvertisementId = c.AdvertisementId,
                    IsReadByBuyer = c.IsReadByBuyer,
                    IsReadBySeller = c.IsReadBySeller,
                    IsArchivedByBuyer = c.IsArchivedByBuyer,
                    IsArchivedBySeller = c.IsArchivedBySeller,
                    StartedOn = c.StartedOn
                })
                .ToListAsync();
        }

        public async Task<bool> ArchiveAsync(string conversationId, string userId)
        {
            if (!await context.Conversation.AnyAsync(c => c.Id == conversationId))
            {
                return false;
            }

            var conversation = await context.Conversation.SingleOrDefaultAsync(c => c.Id == conversationId);

            if (conversation.BuyerId == userId)
            {
                conversation.IsArchivedByBuyer = true;
            }
            else if (conversation.SellerId == userId)
            {
                conversation.IsArchivedBySeller = true;
            }
            else
            {
                return false;
            }

            context.Conversation.Update(conversation);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<int> ArchiveAllAsync(string userId)
        {
            var conversations = await GetAllByUserIdAsync(userId);

            if (conversations == null)
            {
                return 0;
            }

            foreach (var conversation in conversations)
            {
                await ArchiveAsync(conversation.Id, userId);
            }

            return conversations.Count();
        }
    }
}
