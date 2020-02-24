﻿using Microsoft.AspNetCore.Mvc;

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
                ReportedOn = null
            };

            await context.Advertisements.AddAsync(ad);
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
                    .OrderByDescending(a => a.Price)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();

            }
            else if (orderBy == "priceAsc")
            {
                orderedAds = await ads
                    .OrderBy(a => a.Price)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();
            }
            else if (orderBy == "dateAsc")
            {
                orderedAds = await ads
                    .OrderBy(a => a.CreatedOn)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();
            }
            else
            {
                orderedAds = await ads
                    .OrderByDescending(a => a.CreatedOn)
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
                    Price = a.Price
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
                    .OrderByDescending(a => a.Price)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();

            }else if (orderBy == "priceAsc")
            {
                orderedAds = await ads
                    .OrderBy(a => a.Price)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();
            }else if (orderBy == "dateAsc")
            {
                orderedAds = await ads
                    .OrderBy(a => a.CreatedOn)
                    .Take(page * adsPerPage)
                    .Skip((page - 1) * adsPerPage)
                    .ToListAsync();
            }else
            {
                orderedAds = await ads
                    .OrderByDescending(a => a.CreatedOn)
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
                    Price = a.Price
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
                    Price = a.Price
                })
                .ToList();

            return result;
        }

        public async Task<int> GetCountByCategoryIdAsync(string categoryId)
        {
            return await context.Advertisements
                .CountAsync(a => (a.CategoryId == categoryId || a.SubCategoryId == categoryId) && a.IsArchived == false);
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
                Price = ad.Price
            };
        }
    }
}
