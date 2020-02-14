namespace Shoplify.Web.ViewModels.Town
{
    using Shoplify.Services.Mapping;
    using Shoplify.Services.Models;

    public class TownDropdownViewModel : IMapFrom<TownServiceModel>, IMapTo<TownServiceModel>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
