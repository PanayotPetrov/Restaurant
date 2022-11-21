namespace Restaurant.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Restaurant.Common;
    using Restaurant.Data.Models;

    public class AdminSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var adminManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string adminName = "ppetrov20";
            await SeedAdminAsync(adminManager, adminName, GlobalConstants.AdministratorRoleName);
        }

        private static async Task SeedAdminAsync(UserManager<ApplicationUser> adminManager, string adminName, string roleName)
        {
            var admin = await adminManager.FindByNameAsync(adminName);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminName,
                    FirstName = "Pesho",
                    LastName = "Petrov",
                };

                var result = await adminManager.CreateAsync(admin, "123456");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            var result2 = await adminManager.AddToRoleAsync(admin, roleName);
        }
    }
}
