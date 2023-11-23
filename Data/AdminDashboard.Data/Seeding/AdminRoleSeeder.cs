namespace AdminDashboard.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using AdminDashboard.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Restaurant.Data.Common;

    public class AdminRoleSeeder : ISeeder<AdminDashboardDbContext>
    {
        public async Task SeedAsync(AdminDashboardDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AdminDashboardRole>>();

            await SeedRoleAsync(roleManager, "Superuser");
            await SeedRoleAsync(roleManager, "Standard");
            await SeedRoleAsync(roleManager, "Readonly");
        }

        private static async Task SeedRoleAsync(RoleManager<AdminDashboardRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new AdminDashboardRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
