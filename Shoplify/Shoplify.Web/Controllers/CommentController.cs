namespace Shoplify.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Services.Interfaces;

    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService commentService;

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        public async Task<IActionResult> GetAllByAd(string id)
        {
            var comments = await commentService.GetAllByAdIdAsync(id);

            return Json(comments);
        }
    }
}
