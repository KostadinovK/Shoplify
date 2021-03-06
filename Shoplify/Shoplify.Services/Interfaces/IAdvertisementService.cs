﻿namespace Shoplify.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Services.Models;
    using Shoplify.Services.Models.Advertisement;

    public interface IAdvertisementService
    {
        Task CreateAsync(AdvertisementCreateServiceModel advertisement);

        Task EditAsync(AdvertisementEditServiceModel advertisement);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetByCategoryIdAsync(string categoryId, int page, int adsPerPage, string orderBy);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetByUserIdAsync(string userId, int page, int adsPerPage, string orderBy);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetBySearchAsync(string search, int page, int adsPerPage, string orderBy);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetLatestAsync(int count, string userId);

        Task<int> GetCountByCategoryIdAsync(string categoryId);

        Task<int> GetCountByUserIdAsync(string userId);

        Task<int> GetCountBySearchAsync(string search);

        bool Contains(string adId);

        Task<AdvertisementViewServiceModel> GetByIdAsync(string id);

        Task ArchiveByIdAsync(string id);

        Task UnarchiveByIdAsync(string id);

        Task PromoteByIdAsync(string id, int days);

        Task UnpromoteByIdAsync(string id);

        Task BanByIdAsync(string id);

        Task UnbanByIdAsync(string id);

        Task IncrementViewsAsync(string id);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetBannedAdsByUserIdAsync(string userId, int page);

        Task<int> GetBannedAdsCountByUserIdAsync(string userId);

        Task<IEnumerable<AdvertisementViewServiceModel>> GetArchivedAdsByUserIdAsync(string userId, int page);

        Task<int> GetArchivedAdsCountByUserIdAsync(string userId);

        Task<int> ArchiveAllExpiredAdsAsync(DateTime expirationDate);

        Task<int> UnPromoteAllExpiredAdsAsync(DateTime expirationDate);

        Task<int> GetAllAdsCountAsync();

        Task<IEnumerable<AdvertisementViewByAdminViewModel>> GetAllAdsAsync(int page, int adsPerPage);

        Task<Dictionary<string, int>> GetNewAdsCountByDaysFromThisWeekAsync();

        Task<Dictionary<string, int>> GetAdsCountByCategoriesAsync();
    }
}
