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
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
