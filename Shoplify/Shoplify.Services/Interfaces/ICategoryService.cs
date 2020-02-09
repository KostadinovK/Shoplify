namespace Shoplify.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Services.Models;

    public interface ICategoryService
    {
        Task<bool> CreateAsync(CategoryServiceModel categoryServiceModel);

        Task<bool> CreateAllAsync(IList<string> names, IList<string> cssIcons = null);
    }
}
