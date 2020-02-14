using System.Collections.Generic;
using Shoplify.Web.BindingModels.Advertisement;
using Shoplify.Web.ViewModels.Town;

namespace Shoplify.Web.ViewModels.Advertisement
{
    using Category;

    public class CreateViewModel
    {
        public CreateAdvertisementBindingModel BindingModel { get; set; }

        public IEnumerable<CategoryDropdownViewModel> Categories { get; set; }

        public IEnumerable<TownDropdownViewModel> Towns { get; set; }
    }
}
