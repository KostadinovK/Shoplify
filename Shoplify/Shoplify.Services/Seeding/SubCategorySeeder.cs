using Shoplify.Services.Models;

namespace Shoplify.Services.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.Extensions.DependencyInjection;
    using Shoplify.Data.Seeding;
    using Shoplify.Services.Interfaces;
    using Shoplify.Web.Data;

    class SubCategorySeeder : ISeeder
    {
        public async Task<bool> SeedAsync(ShoplifyDbContext context, IServiceProvider serviceProvider)
        {
            if (context.SubCategories.Any())
            {
                return false;
            }

            var categoryService = serviceProvider.GetRequiredService<ICategoryService>();
            var subCategoryService = serviceProvider.GetRequiredService<ISubCategoryService>();

            var categoriesWithSubCategories = new Dictionary<string, List<string>>()
            {
                {"Real Estates", new List<string>() {"Sales", "Rents", "Roommate"}},
                {"Animals", new List<string>() {"Dogs", "Cats", "Fish", "Birds", "Goods", "Other"}},
                {"Gifts", new List<string>()},
                {
                    "Vehicles",
                    new List<string>() {"Cars/Car Parts", "Motorbikes/Parts", "Tires", "ATMs", "Mopeds", "Other"}
                },
                {"Home", new List<string>() {"Furniture", "Household", "Cleaning", "Other"}},
                {"Garden", new List<string>() {"Plants", "Trees", "Tools", "Seeds", "Other"}},
                {"Services", new List<string>() {"Business", "Beauty", "Courses", "Other"}},
                {
                    "Electronics",
                    new List<string>()
                        {"Computers", "Phones", "Photo", "Audio", "Tablets", "TVs", "Navigation", "Coolers", "Other"}
                },
                {"Sport", new List<string>() {"Football", "Basketball", "Volleyball", "Swimming", "Other"}},
                {"Books", new List<string>() {"Sci-Fi", "Technical", "SchoolBooks", "Other"}},
                {"Hobby", new List<string>() {"Games", "Music", "Films", "Board games", "Playing Cards", "Other"}},
                {"Fashion", new List<string>() {"Clothes", "Perfumes", "Jewelry", "Shoes", "Watches", "Other"}},
            };

            foreach (var kvp in categoriesWithSubCategories)
            {
                var category = context.Categories.SingleOrDefault(c => c.Name == kvp.Key);

                await subCategoryService.CreateAllAsync(kvp.Value, category.Id);
            }

            var result = await context.SaveChangesAsync();

            return result > 0;
        }
    }
}
