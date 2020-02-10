using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shoplify.Domain;

namespace Shoplify.Services.Implementations
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Interfaces;
    using Models;
    using Shoplify.Web.Data;

    public class SubCategoryService : ISubCategoryService
    {
        private const string NullOrEmptyNameErrorMessage = "SubCategory Name is null or empty.";
        private const string InvalidCategoryIdErrorMessage = "Invalid Category Id.";
        private const string NullCategoryNamesListErrorMessage = "SubCategories names list is null.";
        private const string InvalidIdErrorMessage = "Subcategory with this Id doesn't exist";
        private const string InvalidNameErrorMessage = "Subcategory with this Name doesn't exist";

        private ShoplifyDbContext context;
        private ICategoryService categoryService;

        public SubCategoryService(ShoplifyDbContext context, ICategoryService categoryService)
        {
            this.context = context;
            this.categoryService = categoryService;
        }

        public async Task<bool> CreateAsync(SubCategoryServiceModel subCategoryServiceModel)
        {
            var subCategory = new SubCategory()
            {
                Name = subCategoryServiceModel.Name,
                CategoryId = subCategoryServiceModel.CategoryId
            };

            if (string.IsNullOrEmpty(subCategory.Name) ||
                string.IsNullOrWhiteSpace(subCategory.Name))
            {
                throw new ArgumentNullException(NullOrEmptyNameErrorMessage);
            }

            if (!await categoryService.ContainsByIdAsync(subCategoryServiceModel.CategoryId))
            {
                throw new ArgumentNullException(InvalidCategoryIdErrorMessage);
            }

            await context.SubCategories.AddAsync(subCategory);

            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> CreateAllAsync(IList<string> names, string categoryId)
        {
            if (names == null)
            {
                throw new ArgumentNullException(NullCategoryNamesListErrorMessage);
            }

            for (int i = 0; i < names.Count; i++)
            {
                var subCategoryServiceModel = new SubCategoryServiceModel()
                {
                    Name = names[i],
                    CategoryId = categoryId
                };

                await CreateAsync(subCategoryServiceModel);
            }

            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> ContainsByIdAsync(string id)
        {
            var result = await context.SubCategories.SingleOrDefaultAsync(s => s.Id == id) != null;

            return result;
        }

        public async Task<SubCategoryServiceModel> GetByIdAsync(string id)
        {
            if (!await ContainsByIdAsync(id))
            {
                throw new ArgumentException(InvalidIdErrorMessage);
            }

            var subCategory = await context.SubCategories.SingleOrDefaultAsync(s => s.Id == id);

            var subCategoryServiceModel = new SubCategoryServiceModel()
            {
                Id = subCategory.Id,
                Name = subCategory.Name,
                CategoryId = subCategory.CategoryId
            };

            return subCategoryServiceModel;
        }

        public async Task<SubCategoryServiceModel> GetByNameAsync(string name)
        {
            var subCategory = await context.SubCategories.SingleOrDefaultAsync(s => s.Name == name);

            if (subCategory == null)
            {
                throw new ArgumentException(InvalidNameErrorMessage);
            }

            var subCategoryServiceModel = new SubCategoryServiceModel()
            {
                Id = subCategory.Id,
                Name = subCategory.Name,
                CategoryId = subCategory.CategoryId
            };

            return subCategoryServiceModel;
        }

        public IQueryable<SubCategoryServiceModel> GetAllByCategoryId(string categoryId)
        {
            return context.SubCategories
                .Where(s => s.CategoryId == categoryId)
                .Select(s => new SubCategoryServiceModel()
                {
                    Id = s.Id,
                    Name = s.Name,
                    CategoryId = s.CategoryId,
                });
        }
    }
}
