using Shoplify.Domain;

namespace Shoplify.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Interfaces;
    using Models;
    using Shoplify.Web.Data;

    public class CategoryService : ICategoryService
    {
        private const string NullOrEmptyNameErrorMessage = "Category name is null or empty.";
        private const string NullCategoryNamesListErrorMessage = "Category names list is null.";
        private const string InvalidCategoryIconList = "Category icons list count must be equal to category names list count.";

        private ShoplifyDbContext context;

        public CategoryService(ShoplifyDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreateAsync(CategoryServiceModel categoryServiceModel)
        {
            var category = new Category
            {
                Name = categoryServiceModel.Name,
                CssIconClass = categoryServiceModel.CssIconClass
            };

            if (string.IsNullOrEmpty(category.Name) ||
                string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentNullException(NullOrEmptyNameErrorMessage);
            }

            await context.Categories.AddAsync(category);

            var result = await context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CreateAllAsync(IList<string> names, IList<string> cssIcons = null)
        {
            if (names == null)
            {
                throw new ArgumentNullException(NullCategoryNamesListErrorMessage);
            }

            if (cssIcons != null && cssIcons.Count != 0 && cssIcons.Count != names.Count)
            {
                throw new ArgumentNullException(InvalidCategoryIconList);
            }

            for (int i = 0; i < names.Count; i++)
            {
                var categoryServiceModel = new CategoryServiceModel
                {
                    Name = names[i],
                    CssIconClass = null
                };

                if (cssIcons != null)
                {
                    categoryServiceModel.CssIconClass = cssIcons[i];
                }

                await CreateAsync(categoryServiceModel);
            }

            var result = await context.SaveChangesAsync();

            return result > 0;
        }
    }
}
