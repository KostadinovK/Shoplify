﻿namespace Shoplify.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models.Comment;
    using Shoplify.Web.BindingModels.Comment;

    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService commentService;
        private readonly UserManager<User> userManager;

        public CommentController(ICommentService commentService, UserManager<User> userManager)
        {
            this.commentService = commentService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> GetAllByAd(string id)
        {
            var comments = await commentService.GetAllByAdIdAsync(id);

            return Json(comments);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateBindingModel input)
        {
            if (!ModelState.IsValid)
            {
                return Redirect($"/Advertisement/Details?id={input.AdvertisementId}");
            }

            var serviceModel = new CreateServiceModel
            {
                Text = input.Text,
                AdvertisementId = input.AdvertisementId,
                WrittenOn = DateTime.UtcNow,
                UserId = userManager.GetUserAsync(HttpContext.User).GetAwaiter().GetResult().Id
            };

            await commentService.PostAsync(serviceModel);

            return Redirect($"/Advertisement/Details?id={input.AdvertisementId}");
        }
    }
}