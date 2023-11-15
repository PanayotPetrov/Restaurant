namespace AdminDashboard.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Restaurant.Data.Common;

    public class AdminDashboardDbContextSeeder : ISeeder<AdminDashboardDbContext>
    {
        public async Task SeedAsync(AdminDashboardDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(AdminDashboardDbContext));

            var seeders = new List<ISeeder<AdminDashboardDbContext>>
                          {
                              new AdminRoleSeeder(),
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }
        }
    }
}
