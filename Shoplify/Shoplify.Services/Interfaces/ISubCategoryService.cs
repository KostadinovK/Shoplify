namespace Shoplify.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Models;

    public interface ISubCategoryService
    {
        Task<bool> CreateAsync(SubCategoryServiceModel subCategoryServiceModel);

        Task<bool> CreateAllAsync(IList<string> names, string categoryId);
    }
}
