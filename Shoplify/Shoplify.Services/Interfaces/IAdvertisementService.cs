﻿namespace Shoplify.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Services.Models;
    using Shoplify.Services.Models.Advertisement;

    public interface IAdvertisementService
    {
        Task CreateAsync(AdvertisementCreateServiceModel advertisement);

        Task EditAsync(AdvertisementEditServiceModel advertisement);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetByCategoryIdAsync(string categoryId, int page, int adsPerPage, string orderBy);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetBySearchAsync(string search, int page, int adsPerPage, string orderBy);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetLatestAsync(int count, string userId);

        Task<int> GetCountByCategoryIdAsync(string categoryId);

        Task<int> GetCountBySearchAsync(string search);

        bool Contains(string adId);

        Task<AdvertisementViewServiceModel> GetByIdAsync(string id);

        Task ArchiveByIdAsync(string id);
    }
}
