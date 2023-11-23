namespace Restaurant.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Restaurant.Data.Common;
    using Restaurant.Data.Models;

    public class UserMessageCategorySeeder : ISeeder<ApplicationDbContext>
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.UserMessageCategories.Any())
            {
                return;
            }

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Order related",
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Reservation related",
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Menu related",
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Account related",
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Careers",
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Feedback",
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Other...",
            });
        }
    }
}
