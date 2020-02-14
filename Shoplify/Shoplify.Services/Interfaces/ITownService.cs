namespace Shoplify.Services.Interfaces
{
    using System.Linq;
    using System.Threading.Tasks;

    using Models;

    public interface ITownService
    {
        Task<TownServiceModel> GetByIdAsync(string id);

        Task<TownServiceModel> GetByNameAsync(string name);

        IQueryable<TownServiceModel> GetAll();
    }
}
