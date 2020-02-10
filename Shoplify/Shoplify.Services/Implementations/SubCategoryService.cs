using System;
using System.Linq;
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
            throw new System.NotImplementedException();
        }
    }
}
