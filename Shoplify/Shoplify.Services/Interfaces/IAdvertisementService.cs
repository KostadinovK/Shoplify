namespace Shoplify.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Services.Models;

    public interface IAdvertisementService
    {
        Task CreateAsync(AdvertisementCreateServiceModel advertisement);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetByCategoryIdAsync(string categoryId, int page);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetBySearchAsync(string search, int page);
    }
}
