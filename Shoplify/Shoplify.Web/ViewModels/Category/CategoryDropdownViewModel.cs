using Shoplify.Services.Models;

namespace Shoplify.Web.ViewModels.Category
{
    using Shoplify.Services.Mapping;

    public class CategoryDropdownViewModel : IMapFrom<CategoryServiceModel>, IMapTo<CategoryServiceModel>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
