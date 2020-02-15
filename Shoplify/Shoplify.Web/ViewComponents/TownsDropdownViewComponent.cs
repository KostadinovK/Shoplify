namespace Shoplify.Web.ViewComponents
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Services.Interfaces;

    [ViewComponent(Name = "TownsDropdown")]
    public class TownsDropdownViewComponent : ViewComponent
    {
        private readonly ITownService townService;

        public TownsDropdownViewComponent(ITownService townService)
        {
            this.townService = townService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var towns = townService.GetAll().OrderBy(t => t.Name).ToList();

            return View(towns);
        }
    }
}
