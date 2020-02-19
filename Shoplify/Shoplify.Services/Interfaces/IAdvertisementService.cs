namespace Shoplify.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Services.Models;

    public interface IAdvertisementService
    {
        Task CreateAsync(AdvertisementCreateServiceModel advertisement);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetAllByCategoryIdAsync(string categoryId);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetAllBySearchAsync(string search);
    }
}
