using Shoplify.Services.Interfaces;
using Shoplify.Web.Areas.Administration.ViewModels.Panel;

namespace Shoplify.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Common;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class PanelController : Controller
    {
        private readonly IUserService userService;

        public PanelController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var usersCount = await userService.GetAllUserCountWithoutAdminAsync();

            var viewModel = new PanelViewModel
            {
                UsersCount = usersCount,
            };

            return View(viewModel);
        }
    }
}
