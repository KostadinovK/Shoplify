namespace Shoplify.Services.Models
{
    using Shoplify.Domain;
    using Shoplify.Services.Mapping;

    public class CategoryServiceModel : IMapFrom<Category>, IMapTo<Category>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CssIconClass { get; set; }
    }
}
