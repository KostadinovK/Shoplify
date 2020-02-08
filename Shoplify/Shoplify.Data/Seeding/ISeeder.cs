namespace Shoplify.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using Shoplify.Web.Data;

    public interface ISeeder
    {
        Task<int> SeedAsync(ShoplifyDbContext context, IServiceProvider serviceProvider);
    }
}
