namespace Shoplify.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Common;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.Areas.Administration.ViewModels.User;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> All(int page = 1, string orderBy = "nameAsc")
        {
            if (page <= 0)
            {
                return Redirect("/Panel/Index");
            }

            var usersCount = await userService.GetAllUserCountWithoutAdminAsync();
            var lastPage = usersCount / GlobalConstants.UsersOnPageCount + 1;

            if (usersCount % GlobalConstants.UsersOnPageCount == 0 && usersCount > 0)
            {
                lastPage -= 1;
            }

            if (page > lastPage)
            {
                return Redirect("/Panel/Index");
            }

            var users = await userService.GetAllUsersWithoutAdminAsync(page, GlobalConstants.UsersOnPageCount, orderBy);

            var viewModel = new UserListingPageViewModel
            {
                TotalUsersCount = usersCount,
                CurrentPage = page,
                LastPage = lastPage,
                OrderParam = $"orderBy={orderBy}",
                Users = new List<UserViewModel>()
            };

            foreach (var user in users)
            {
                viewModel.Users.Add(new UserViewModel
                {
                    BannedOn = user.BannedOn.ToLocalTime().ToString(GlobalConstants.DateTimeFormat),
                    Id = user.Id,
                    IsBanned = user.IsBanned,
                    RegisteredOn = user.RegisteredOn.ToLocalTime().ToString(GlobalConstants.DateTimeFormat),
                    Username = user.Username
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Unban(string userId)
        {
            await userService.UnbanUserByIdAsync(userId);

            return RedirectToAction("All");
        }

        public async Task<IActionResult> Ban(string userId)
        {
           await userService.BanUserByIdAsync(userId);

           return RedirectToAction("All");
        }
    }
}
