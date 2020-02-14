namespace Shoplify.Services.Implementations
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Shoplify.Web.Data;

    public class TownService : ITownService
    {
        private const string InvalidIdErrorMessage = "Town with this Id doesn't exist";
        private const string InvalidNameErrorMessage = "Town with this name doesn't exist";

        private ShoplifyDbContext context;

        public TownService(ShoplifyDbContext context)
        {
            this.context = context;
        }

        public async Task<TownServiceModel> GetByIdAsync(string id)
        {
            var town = await context.Towns.SingleOrDefaultAsync(t => t.Id == id);

            if (town == null)
            {
                throw new ArgumentException(InvalidIdErrorMessage);
            }

            var townServiceModel = new TownServiceModel()
            {
                Id = town.Id,
                Name = town.Name,
            };

            return townServiceModel;
        }

        public async Task<TownServiceModel> GetByNameAsync(string name)
        {
            var town = await context.Towns.SingleOrDefaultAsync(t => t.Name == name);

            if (town == null)
            {
                throw new ArgumentException(InvalidNameErrorMessage);
            }

            var townServiceModel = new TownServiceModel()
            {
                Id = town.Id,
                Name = town.Name,
            };

            return townServiceModel;
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
