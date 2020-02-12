using Shoplify.Domain;

namespace Shoplify.Services.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore.Internal;
    using Shoplify.Data.Seeding;
    using Shoplify.Web.Data;

    public class TownSeeder : ISeeder
    {
        public async Task<bool> SeedAsync(ShoplifyDbContext context, IServiceProvider serviceProvider)
        {
            if (context.Towns.Any())
            {
                return false;
            }

            var townNames = new List<string>()
            {
                "Sofia",
                "Varna",
                "Plovdiv",
                "Burgas",
                "Pernik",
                "Stara Zagora",
                "Veliko Tarnovo",
                "Shumen",
                "Montana",
                "Vidin",
                "Vratza"
            };

            foreach (var name in townNames)
            {
                await context.Towns.AddAsync(new Town() { Name = name });
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
