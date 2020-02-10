namespace Shoplify.Services.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shoplify.Data.Seeding;
    using Shoplify.Web.Data;

    public class ShoplifyDbContextSeeder : ISeeder
    {
        public async Task<bool> SeedAsync(ShoplifyDbContext context, IServiceProvider serviceProvider)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var seeders = new List<ISeeder>()
            {
                new UserRoleSeeder(),
                new CategorySeeder()
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(context, serviceProvider);
                await context.SaveChangesAsync();
            }

            return true;
        }
    }
}
