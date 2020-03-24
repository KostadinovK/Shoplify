namespace Shoplify.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Shoplify.Common;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models;
    using Shoplify.Services.Models.Advertisement;
    using Shoplify.Web.Data;

    public class AdvertisementService : IAdvertisementService
    {
        private const string InvalidAdId = "Advertisement Id is invalid.";

        private ShoplifyDbContext context;
        private ICloudinaryService cloudinaryService;

        public AdvertisementService(ShoplifyDbContext context, ICloudinaryService cloudinaryService)
        {
            this.context = context;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task CreateAsync(AdvertisementCreateServiceModel advertisement)
        {
            List<string> imageUrls = new List<string>();

            if (advertisement.Images != null && advertisement.Images.Count != 0)
            {
                imageUrls = advertisement.Images
                    .Select(async x =>
                        await cloudinaryService.UploadPictureAsync(x, x.FileName))
                    .Select(x => x.Result)
                    .ToList();
            }

            var ad = new Advertisement
            {
                Name = advertisement.Name,
                Address = advertisement.Address,
                ArchivedOn = DateTime.UtcNow.AddDays(GlobalConstants.AdvertisementDurationDays),
                CategoryId = advertisement.CategoryId,
                SubCategoryId = advertisement.SubCategoryId,
                Condition = advertisement.Condition,
                CreatedOn = DateTime.UtcNow,
                Price = advertisement.Price,
                Description = advertisement.Description,
                UserId = advertisement.UserId,
                Images = string.Join(GlobalConstants.ImageUrlInDatabaseSeparator, imageUrls),
                EditedOn = null,
                IsArchived = false,
                TownId = advertisement.TownId,
                Number = advertisement.Number,
                IsReported = false,
                ReportedOn = null,
                IsBanned = false,
                BannedOn = null,
                IsPromoted = false,
                PromotedOn = null,
                PromotedUntil = null
            };

            await context.Advertisements.AddAsync(ad);
            await context.SaveChangesAsync();
        }

        public async Task EditAsync(AdvertisementEditServiceModel advertisement)
        {
            List<string> imageUrls = new List<string>();

            if (advertisement.Images != null && advertisement.Images.Count != 0)
            {
                imageUrls = advertisement.Images
                    .Select(async x =>
                        await cloudinaryService.UploadPictureAsync(x, x.FileName))
                    .Select(x => x.Result)
                    .ToList();
            }

            var advertisementFromDb = context.Advertisements.SingleOrDefault(a => a.Id == advertisement.Id);

            if (advertisementFromDb == null)
            {
                return;
            }

            advertisementFromDb.Name = advertisement.Name;
            advertisementFromDb.Address = advertisement.Address;
            advertisementFromDb.CategoryId = advertisement.CategoryId;
            advertisementFromDb.SubCategoryId = advertisement.SubCategoryId;
            advertisementFromDb.Condition = advertisement.Condition;
            advertisementFromDb.Price = advertisement.Price;
            advertisementFromDb.Description = advertisement.Description;

            if (imageUrls.Count != 0)
            {
                advertisementFromDb.Images = string.Join(GlobalConstants.ImageUrlInDatabaseSeparator, imageUrls);
            }

            advertisementFromDb.EditedOn = DateTime.UtcNow;
            advertisementFromDb.TownId = advertisement.TownId;
            advertisementFromDb.Number = advertisement.Number;

            context.Advertisements.Update(advertisementFromDb);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AdvertisementViewServiceModel>> GetByCategoryIdAsync(string categoryId, int page, int adsPerPage, string orderBy)
        {
            var ads = context.Advertisements
                .Where(a => (a.CategoryId == categoryId || a.SubCategoryId == categoryId) && a.IsArchived == false);

            var orderedAds = new List<Advertisement>();

            if (orderBy == "priceDesc")
            {
                orderedAds = await ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenByDescending(a => a.Price)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();

            }
            else if (orderBy == "priceAsc")
            {
                orderedAds = await ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenBy(a => a.Price)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();
            }
            else if (orderBy == "dateAsc")
            {
                orderedAds = await ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenBy(a => a.CreatedOn)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();
            }
            else
            {
                orderedAds = await ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenByDescending(a => a.CreatedOn)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();
            }

            var result = orderedAds.Select(a => new AdvertisementViewServiceModel
                {
                    Address = a.Address,
                    CategoryId = a.CategoryId,
                    Condition = a.Condition,
                    CreatedOn = a.CreatedOn.ToLocalTime(),
                    Description = a.Description,
                    Id = a.Id,
                    Images = a.Images.Split(GlobalConstants.ImageUrlInDatabaseSeparator).ToList(),
                    SubCategoryId = a.SubCategoryId,
                    Name = a.Name,
                    Number = a.Number,
                    TownId = a.TownId,
                    UserId = a.UserId,
                    Price = a.Price,
                    IsArchived = a.IsArchived,
                    ArchivedOn = a.ArchivedOn.GetValueOrDefault().ToLocalTime(),
                    IsBanned = a.IsBanned,
                    BannedOn = a.BannedOn.GetValueOrDefault().ToLocalTime(),
                    IsPromoted = a.IsPromoted,
                    PromotedOn = a.PromotedOn.GetValueOrDefault().ToLocalTime(),
                    PromotedUntil = a.PromotedUntil.GetValueOrDefault().ToLocalTime(),
                    Views = a.Views
            })
                .ToList();

            return result;
        }

        public async Task<IEnumerable<AdvertisementViewServiceModel>> GetByUserIdAsync(string userId, int page, int adsPerPage, string orderBy)
        {
            var ads = context.Advertisements
                .Where(a => (a.UserId == userId) && a.IsArchived == false);

            var orderedAds = new List<Advertisement>();

            if (orderBy == "priceDesc")
            {
                orderedAds = await ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenByDescending(a => a.Price)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();

            }
            else if (orderBy == "priceAsc")
            {
                orderedAds = await ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenBy(a => a.Price)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();
            }
            else if (orderBy == "dateAsc")
            {
                orderedAds = await ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenBy(a => a.CreatedOn)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();
            }
            else
            {
                orderedAds = await ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenByDescending(a => a.CreatedOn)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();
            }

            var result = orderedAds.Select(a => new AdvertisementViewServiceModel
            {
                Address = a.Address,
                CategoryId = a.CategoryId,
                Condition = a.Condition,
                CreatedOn = a.CreatedOn.ToLocalTime(),
                Description = a.Description,
                Id = a.Id,
                Images = a.Images.Split(GlobalConstants.ImageUrlInDatabaseSeparator).ToList(),
                SubCategoryId = a.SubCategoryId,
                Name = a.Name,
                Number = a.Number,
                TownId = a.TownId,
                UserId = a.UserId,
                Price = a.Price,
                IsArchived = a.IsArchived,
                ArchivedOn = a.ArchivedOn.GetValueOrDefault().ToLocalTime(),
                IsBanned = a.IsBanned,
                BannedOn = a.BannedOn.GetValueOrDefault().ToLocalTime(),
                IsPromoted = a.IsPromoted,
                PromotedOn = a.PromotedOn.GetValueOrDefault().ToLocalTime(),
                PromotedUntil = a.PromotedUntil.GetValueOrDefault().ToLocalTime(),
                Views = a.Views
            })
                .ToList();

            return result;
        }

        public async Task<IEnumerable<AdvertisementViewServiceModel>> GetBySearchAsync(string search, int page, int adsPerPage, string orderBy)
        {
            var ads = context.Advertisements
                .Where(a => a.Name.ToLower().Contains(search.ToLower()) && a.IsArchived == false);

            var orderedAds = new List<Advertisement>();

            if (orderBy == "priceDesc")
            {
                orderedAds = await ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenByDescending(a => a.Price)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();

            }
            else if (orderBy == "priceAsc")
            {
                orderedAds = await ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenBy(a => a.Price)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();
            }
            else if (orderBy == "dateAsc")
            {
                orderedAds = await ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenBy(a => a.CreatedOn)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();
            }
            else
            {
                orderedAds = await ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenByDescending(a => a.CreatedOn)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();
            }

            var result = orderedAds.Select(a => new AdvertisementViewServiceModel
                {
                    Address = a.Address,
                    CategoryId = a.CategoryId,
                    Condition = a.Condition,
                    CreatedOn = a.CreatedOn.ToLocalTime(),
                    Description = a.Description,
                    Id = a.Id,
                    Images = a.Images.Split(GlobalConstants.ImageUrlInDatabaseSeparator).ToList(),
                    SubCategoryId = a.SubCategoryId,
                    Name = a.Name,
                    Number = a.Number,
                    TownId = a.TownId,
                    UserId = a.UserId,
                    Price = a.Price,
                    IsArchived = a.IsArchived,
                    ArchivedOn = a.ArchivedOn.GetValueOrDefault().ToLocalTime(),
                    IsBanned = a.IsBanned,
                    BannedOn = a.BannedOn.GetValueOrDefault().ToLocalTime(),
                    IsPromoted = a.IsPromoted,
                    PromotedOn = a.PromotedOn.GetValueOrDefault().ToLocalTime(),
                    PromotedUntil = a.PromotedUntil.GetValueOrDefault().ToLocalTime(),
                    Views = a.Views
            })
                .ToList();

            return result;
        }

        public async Task<IEnumerable<AdvertisementViewServiceModel>> GetLatestAsync(int count, string userId)
        {
            var ads = await context.Advertisements
                .Where(a => a.UserId != userId && a.IsArchived == false)
                .OrderByDescending(a => a.CreatedOn)
                .Take(count)
                .ToListAsync();

            var result = ads.Select(a => new AdvertisementViewServiceModel
                {
                    Address = a.Address,
                    CategoryId = a.CategoryId,
                    Condition = a.Condition,
                    CreatedOn = a.CreatedOn.ToLocalTime(),
                    Description = a.Description,
                    Id = a.Id,
                    Images = a.Images.Split(GlobalConstants.ImageUrlInDatabaseSeparator).ToList(),
                    SubCategoryId = a.SubCategoryId,
                    Name = a.Name,
                    Number = a.Number,
                    TownId = a.TownId,
                    UserId = a.UserId,
                    Price = a.Price,
                    IsArchived = a.IsArchived,
                    ArchivedOn = a.ArchivedOn.GetValueOrDefault().ToLocalTime(),
                    IsBanned = a.IsBanned,
                    BannedOn = a.BannedOn.GetValueOrDefault().ToLocalTime(),
                    IsPromoted = a.IsPromoted,
                    PromotedOn = a.PromotedOn.GetValueOrDefault().ToLocalTime(),
                    PromotedUntil = a.PromotedUntil.GetValueOrDefault().ToLocalTime(),
                    Views = a.Views
            })
                .ToList();

            return result;
        }

        public async Task<int> GetCountByCategoryIdAsync(string categoryId)
        {
            return await context.Advertisements
                .CountAsync(a => (a.CategoryId == categoryId || a.SubCategoryId == categoryId) && a.IsArchived == false);
        }

        public async Task<int> GetCountByUserIdAsync(string userId)
        {
            return await context.Advertisements
                .CountAsync(a => (a.UserId == userId) && a.IsArchived == false);
        }

        public async Task<int> GetCountBySearchAsync(string search)
        {
            return await context.Advertisements
                .CountAsync(a => a.Name.ToLower().Contains(search.ToLower()) && a.IsArchived == false);
        }

        public bool Contains(string adId)
        {
            return context.Advertisements.Any(a => a.Id == adId);
        }

        public async Task<AdvertisementViewServiceModel> GetByIdAsync(string id)
        {
            if (!Contains(id))
            {
                throw new ArgumentException(InvalidAdId);
            }

            var ad = await context.Advertisements.FirstOrDefaultAsync(a => a.Id == id);

            return new AdvertisementViewServiceModel()
            {
                Address = ad.Address,
                CategoryId = ad.CategoryId,
                Condition = ad.Condition,
                CreatedOn = ad.CreatedOn.ToLocalTime(),
                Description = ad.Description,
                Id = ad.Id,
                Images = ad.Images.Split(GlobalConstants.ImageUrlInDatabaseSeparator).ToList(),
                SubCategoryId = ad.SubCategoryId,
                Name = ad.Name,
                Number = ad.Number,
                TownId = ad.TownId,
                UserId = ad.UserId,
                Price = ad.Price,
                IsArchived = ad.IsArchived,
                ArchivedOn = ad.ArchivedOn.GetValueOrDefault().ToLocalTime(),
                IsBanned = ad.IsBanned,
                BannedOn = ad.BannedOn.GetValueOrDefault().ToLocalTime(),
                IsPromoted = ad.IsPromoted,
                PromotedOn = ad.PromotedOn.GetValueOrDefault().ToLocalTime(),
                PromotedUntil = ad.PromotedUntil.GetValueOrDefault().ToLocalTime(),
                Views = ad.Views
            };
        }

        public async Task ArchiveByIdAsync(string id)
        {
            if (!Contains(id))
            {
                throw new ArgumentException(InvalidAdId);
            }

            var ad = context.Advertisements.SingleOrDefault(a => a.Id == id);

            ad.IsArchived = true;
            ad.IsPromoted = false;
            ad.PromotedOn = null;
            ad.PromotedUntil = null;
            ad.ArchivedOn = DateTime.UtcNow;

            context.Advertisements.Update(ad);

            await context.SaveChangesAsync();
        }

        public async Task UnarchiveByIdAsync(string id)
        {
            if (!Contains(id))
            {
                throw new ArgumentException(InvalidAdId);
            }

            var ad = context.Advertisements.SingleOrDefault(a => a.Id == id);

            ad.IsArchived = false;
            ad.ArchivedOn = DateTime.UtcNow.AddDays(GlobalConstants.AdvertisementDurationDays);

            context.Advertisements.Update(ad);

            await context.SaveChangesAsync();
        }

        public async Task PromoteByIdAsync(string id, int days)
        {
            if (!Contains(id))
            {
                throw new ArgumentException(InvalidAdId);
            }

            var ad = context.Advertisements.SingleOrDefault(a => a.Id == id);

            ad.IsPromoted = true;
            ad.PromotedOn = DateTime.UtcNow;
            ad.PromotedUntil = ad.PromotedOn.GetValueOrDefault().AddDays(days);

            context.Advertisements.Update(ad);

            await context.SaveChangesAsync();
        }

        public async Task UnpromoteByIdAsync(string id)
        {
            if (!Contains(id))
            {
                throw new ArgumentException(InvalidAdId);
            }

            var ad = context.Advertisements.SingleOrDefault(a => a.Id == id);

            ad.IsPromoted = false;
            ad.PromotedOn = null;
            ad.PromotedUntil = null;

            context.Advertisements.Update(ad);

            await context.SaveChangesAsync();
        }

        //Also archivates the ad
        public async Task BanByIdAsync(string id)
        {
            if (!Contains(id))
            {
                throw new ArgumentException(InvalidAdId);
            }

            var ad = context.Advertisements.SingleOrDefault(a => a.Id == id);

            ad.IsBanned = true;
            ad.BannedOn = DateTime.UtcNow;

            context.Advertisements.Update(ad);

            await context.SaveChangesAsync();
        }

        public async Task UnbanByIdAsync(string id)
        {
            if (!Contains(id))
            {
                throw new ArgumentException(InvalidAdId);
            }

            var ad = context.Advertisements.SingleOrDefault(a => a.Id == id);

            ad.IsBanned = false;
            ad.BannedOn = null;

            context.Advertisements.Update(ad);

            await context.SaveChangesAsync();
        }

        public async Task IncrementViewsAsync(string id)
        {
            if (!Contains(id))
            {
                throw new ArgumentException(InvalidAdId);
            }

            var ad = context.Advertisements.SingleOrDefault(a => a.Id == id);

            ad.Views++;

            context.Advertisements.Update(ad);

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AdvertisementViewServiceModel>> GetBannedAdsByUserIdAsync(string userId, int page)
        {
            var ads = await context.Advertisements
                .Where(a => a.UserId == userId && a.IsBanned)
                .OrderBy(a => a.BannedOn)
                .Select(ad =>
                new AdvertisementViewServiceModel
                {
                    CategoryId = ad.CategoryId,
                    CreatedOn = ad.CreatedOn.ToLocalTime(),
                    Id = ad.Id,
                    SubCategoryId = ad.SubCategoryId,
                    Name = ad.Name,
                    UserId = ad.UserId,
                    Price = ad.Price,
                    IsBanned = ad.IsBanned,
                    BannedOn = ad.BannedOn.GetValueOrDefault().ToLocalTime(),
                })
                .Take(page * GlobalConstants.AdsOnPageCount)
                .Skip((page - 1) * GlobalConstants.AdsOnPageCount)
                .ToListAsync();

            return ads;
        }

        public async Task<int> GetBannedAdsCountByUserIdAsync(string userId)
        {
            return await context.Advertisements.CountAsync(a => a.UserId == userId && a.IsBanned);
        }

        public async Task<IEnumerable<AdvertisementViewServiceModel>> GetArchivedAdsByUserIdAsync(string userId, int page)
        {
            var ads = await context.Advertisements
                .Where(a => a.UserId == userId && a.IsArchived)
                .OrderBy(a => a.ArchivedOn)
                .Select(ad =>
                    new AdvertisementViewServiceModel
                    {
                        CategoryId = ad.CategoryId,
                        CreatedOn = ad.CreatedOn.ToLocalTime(),
                        Id = ad.Id,
                        SubCategoryId = ad.SubCategoryId,
                        Name = ad.Name,
                        UserId = ad.UserId,
                        Price = ad.Price,
                        IsArchived = ad.IsArchived,
                        ArchivedOn = ad.ArchivedOn.GetValueOrDefault().ToLocalTime(),
                    })
                .Take(page * GlobalConstants.AdsOnPageCount)
                .Skip((page - 1) * GlobalConstants.AdsOnPageCount)
                .ToListAsync();

            return ads;
        }

        public async Task<int> GetArchivedAdsCountByUserIdAsync(string userId)
        {
            return await context.Advertisements.CountAsync(a => a.UserId == userId && a.IsArchived);
        }

        public async Task<int> ArchiveAllExpiredAdsAsync(DateTime expirationDate)
        {
            var adsToArchive = await context.Advertisements
                .Where(a => a.IsArchived == false && a.ArchivedOn <= expirationDate).ToListAsync();

            foreach (var ad in adsToArchive)
            {
                await ArchiveByIdAsync(ad.Id);
            }

            return adsToArchive.Count;
        }

        public async Task<int> UnPromoteAllExpiredAdsAsync(DateTime expirationDate)
        {
            var adsToUnPromote = await context.Advertisements
                .Where(a => a.IsPromoted && a.PromotedUntil <= expirationDate).ToListAsync();

            foreach (var ad in adsToUnPromote)
            {
                ad.IsPromoted = false;
                ad.PromotedOn = null;
                ad.PromotedUntil = null;

                context.Update(ad);
            }

            await context.SaveChangesAsync();

            return adsToUnPromote.Count;
        }

        public async Task<int> GetAllAdsCountAsync()
        {
            return await context.Advertisements.CountAsync();
        }

        public async Task<IEnumerable<AdvertisementViewByAdminViewModel>> GetAllAdsAsync(int page, int adsPerPage)
        {
            return await context.Advertisements
                .OrderByDescending(a => a.CreatedOn)
                .Select(a => new AdvertisementViewByAdminViewModel
                {
                    Id = a.Id,
                    ArchivedOn = a.ArchivedOn.GetValueOrDefault(),
                    BannedOn = a.BannedOn.GetValueOrDefault(),
                    CreatedOn = a.CreatedOn,
                    Name = a.Name,
                    IsArchived = a.IsArchived,
                    IsPromoted = a.IsPromoted,
                    IsBanned = a.IsBanned,
                    PromotedUntil = a.PromotedUntil.GetValueOrDefault(),
                    UserId = a.UserId,
                    Views = a.Views
                })
                .Take(page * adsPerPage)
                .Skip((page - 1) * adsPerPage)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetNewAdsCountByDaysFromThisWeekAsync()
        {
            var todayDate = DateTime.UtcNow;
            var startOfWeek = todayDate.AddDays(-6);

            var result = new Dictionary<string, int>();

            for (int i = 0; i < 7; i++)
            {
                result.Add(startOfWeek.AddDays(i).DayOfWeek.ToString(), 0);
            }

            var daysOfWeek = await context.Advertisements.Where(a => a.CreatedOn >= startOfWeek && a.CreatedOn <= todayDate)
                .Select(a => a.CreatedOn.DayOfWeek.ToString())
                .ToListAsync();

            foreach (var day in daysOfWeek)
            {
                result[day]++;
            }

            return result;
        }
    }
}
