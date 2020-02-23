namespace Shoplify.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Services.Models;

    public interface IAdvertisementService
    {
        Task CreateAsync(AdvertisementCreateServiceModel advertisement);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetByCategoryIdAsync(string categoryId, int page, int adsPerPage);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetBySearchAsync(string search, int page, int adsPerPage);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetLatestAsync(int count, string userId);

        Task<int> GetCountByCategoryIdAsync(string categoryId);

        Task<int> GetCountBySearchAsync(string search);
    }
}
