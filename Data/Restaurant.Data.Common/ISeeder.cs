namespace Restaurant.Data.Common
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    public interface ISeeder<TDbContext>
        where TDbContext : DbContext
    {
        Task SeedAsync(TDbContext dbContext, IServiceProvider serviceProvider);
    }
}
