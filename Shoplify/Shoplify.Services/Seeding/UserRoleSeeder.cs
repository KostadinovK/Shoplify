using System.Linq;
using Microsoft.AspNetCore.Identity;
using Shoplify.Common;

namespace Shoplify.Services.Seeding
{
    using System;
    using System.Threading.Tasks;

    using Shoplify.Data.Seeding;
    using Shoplify.Web.Data;

    public class UserRoleSeeder : ISeeder
    {
        public async Task<bool> SeedAsync(ShoplifyDbContext context, IServiceProvider serviceProvider)
        {
            if (context.Roles.Any())
            {
                return false;
            }

            await context.Roles.AddAsync(new IdentityRole
            {
                Name = GlobalConstants.AdministratorRoleName,
                NormalizedName = "ADMIN"
            });

            await context.Roles.AddAsync(new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER"
            });

            var result = await context.SaveChangesAsync();

            return result > 0;
        }
    }
}