namespace Shoplify.Web.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shoplify.Services.Interfaces;

    [Authorize]
    public class SubCategoryController : Controller
    {
        private ISubCategoryService subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            this.subCategoryService = subCategoryService;
        }

        public IActionResult GetByCategoryId(string categoryId)
        {
            var subCategories = subCategoryService.GetAllByCategoryId(categoryId).ToList();

            return Json(subCategories);
        }
    }
}
