namespace Shoplify.Services.Implementations
{
    using System.Linq;

    using Interfaces;
    using Models;
    using Shoplify.Web.Data;

    public class TownService : ITownService
    {
        private ShoplifyDbContext context;

        public TownService(ShoplifyDbContext context)
        {
            this.context = context;
        }

        public IQueryable<TownServiceModel> GetAll()
        {
            return context.Towns
                .Select(t => new TownServiceModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                });
        }
    }
}
