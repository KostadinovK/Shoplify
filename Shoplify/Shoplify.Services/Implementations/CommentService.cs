using Microsoft.AspNetCore.Identity;
using Shoplify.Common;
using Shoplify.Domain;

namespace Shoplify.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<CommentServiceModel>> GetAllByAdIdAsync(string id)
        {
            var comments = context.Comments.Where(c => c.AdvertisementId == id).OrderBy(c => c.WrittenOn);

            return await comments.Select(c => new CommentServiceModel()
            {
                Id = c.Id,
                UserId = c.UserId,
                WrittenOn = c.WrittenOn.ToLocalTime().ToString(GlobalConstants.DateTimeFormat),
                Text = c.Text,
                Username = userManager.FindByIdAsync(c.UserId).GetAwaiter().GetResult().UserName
            }).ToListAsync();
        }
    }
}
