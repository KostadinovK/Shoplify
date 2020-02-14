namespace Shoplify.Services.Interfaces
{
    using System.Linq;

    using Models;

    public interface ITownService
    {
        IQueryable<TownServiceModel> GetAll();
    }
}
