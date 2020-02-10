namespace Shoplify.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Shoplify.Services.Models;

    public interface ICategoryService
    {
        Task<bool> CreateAsync(CategoryServiceModel categoryServiceModel);

        Task<bool> CreateAllAsync(IList<string> names, IList<string> cssIcons = null);

        Task<bool> ContainsByIdAsync(string id);

        Task<CategoryServiceModel> GetByIdAsync(string id);

        Task<CategoryServiceModel> GetByNameAsync(string name);

        IQueryable<CategoryServiceModel> GetAll();
    }
}
