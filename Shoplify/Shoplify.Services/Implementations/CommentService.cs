namespace Shoplify.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Shoplify.Common;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models.Comment;
    using Shoplify.Web.Data;

    public class CommentService : ICommentService
    {
        private readonly ShoplifyDbContext context;
        private readonly UserManager<User> userManager;

        public CommentService(ShoplifyDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<ViewServiceModel>> GetAllByAdIdAsync(string id)
        {
            var comments = context.Comments.Where(c => c.AdvertisementId == id).OrderBy(c => c.WrittenOn);

            return await comments.Select(c => new ViewServiceModel()
            {
                Id = c.Id,
                UserId = c.UserId,
                WrittenOn = c.WrittenOn.ToLocalTime().ToString(GlobalConstants.DateTimeFormat),
                Text = c.Text,
                Username = userManager.FindByIdAsync(c.UserId).GetAwaiter().GetResult().UserName
            }).ToListAsync();
        }

        public async Task PostAsync(CreateServiceModel comment)
        {
            var commentForDb = new Comment
            {
                Text = comment.Text,
                AdvertisementId = comment.AdvertisementId,
                UserId = comment.UserId,
                WrittenOn = comment.WrittenOn
            };

            await context.Comments.AddAsync(commentForDb);
            await context.SaveChangesAsync();

            var viewModel = new ViewServiceModel
            {
                Text = commentForDb.Text,
                Id = commentForDb.Id,
                UserId = commentForDb.UserId,
                WrittenOn = commentForDb.WrittenOn.ToString(GlobalConstants.DateTimeFormat),
                Username = userManager.FindByIdAsync(commentForDb.UserId).GetAwaiter().GetResult().UserName
            };
        }
    }
}
