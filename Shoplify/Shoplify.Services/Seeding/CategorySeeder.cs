﻿using Shoplify.Services.Interfaces;

namespace Shoplify.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.Extensions.DependencyInjection;
    using Shoplify.Services.Implementations;
    using Shoplify.Web.Data;

    public class CategorySeeder : ISeeder
    {
        public async Task<bool> SeedAsync(ShoplifyDbContext context, IServiceProvider serviceProvider)
        {
            if (context.Categories.Any())
            {
                return false;
            }

            var categoryService = serviceProvider.GetRequiredService<ICategoryService>();

            var categoryNames = new List<string>()
            {
                "Real Estates",
                "Animals",
                "Gifts",
                "Vehicles",
                "Home",
                "Garden",
                "Services",
                "Electronics",
                "Sport",
                "Books",
                "Hobby",
                "Fashion"
            };

            var categoryCssIcons = new List<string>()
            {
                "fas fa-home",
                "fas fa-paw",
                "fas fa-hand-holding-heart",
                "fas fa-car",
                "fas fa-couch",
                "fas fa-seedling",
                "fas fa-tools",
                "fas fa-mobile-alt",
                "far fa-futbol",
                "fas fa-book",
                "fas fa-gamepad",
                "fas fa-tshirt"
            };

            await categoryService.CreateAllAsync(categoryNames, categoryCssIcons);

            var addedCategoriesCount = await context.SaveChangesAsync();

            return addedCategoriesCount > 0;
        }
    }
}
