namespace Shoplify.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Services.Models.Comment;

    public interface ICommentService
    {
        Task<IEnumerable<CommentServiceModel>> GetAllByAdIdAsync(string id);
    }
}
