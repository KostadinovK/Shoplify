namespace Shoplify.Web.ViewComponents
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Services.Interfaces;

    [ViewComponent(Name = "CategoriesDropdown")]
    public class CategoriesDropdownViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;

        public CategoriesDropdownViewComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = categoryService.GetAll().OrderBy(c => c.Name).ToList();

            return View(categories);
        }
    }
}
