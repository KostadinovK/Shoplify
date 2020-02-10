namespace Shoplify.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Models;

    public interface ISubCategoryService
    {
        Task<bool> CreateAsync(SubCategoryServiceModel subCategoryServiceModel);

        Task<bool> CreateAllAsync(IList<string> names, string categoryId);

        Task<bool> ContainsByIdAsync(string id);

        Task<SubCategoryServiceModel> GetByIdAsync(string id);

        Task<SubCategoryServiceModel> GetByNameAsync(string name);

        IQueryable<SubCategoryServiceModel> GetAllByCategoryId(string categoryId);
    }
}
