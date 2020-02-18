using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Shoplify.Services.Implementations
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Shoplify.Common;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models;
    using Shoplify.Web.Data;

    public class AdvertisementService : IAdvertisementService
    {
        private ShoplifyDbContext context;
        private ICloudinaryService cloudinaryService;

        public AdvertisementService(ShoplifyDbContext context, ICloudinaryService cloudinaryService)
        {
            this.context = context;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task CreateAsync(AdvertisementCreateServiceModel advertisement)
        {
            var imageUrls = advertisement.Images
                .Select(async x =>
                    await cloudinaryService.UploadPictureAsync(x, x.FileName))
                .Select(x => x.Result)
                .ToList();

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

        public async Task<IEnumerable<AdvertisementViewServiceModel>> GetAllByCategoryIdAsync(string categoryId)
        {
            var ads = await context.Advertisements
                .Where(a => a.CategoryId == categoryId || a.SubCategoryId == categoryId).ToListAsync();

            var result = ads.Select(a => new AdvertisementViewServiceModel
                {
                    Address = a.Address,
                    CategoryId = a.CategoryId,
                    Condition = a.Condition,
                    CreatedOn = a.CreatedOn,
                    Description = a.Description,
                    Id = a.Id,
                    Images = a.Images.Split(GlobalConstants.ImageUrlInDatabaseSeparator).ToList(),
                    SubCategoryId = a.SubCategoryId,
                    Name = a.Name,
                    Number = a.Number
                })
                .ToList();

            return result;
        }
    }
}
