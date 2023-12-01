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
                Name = "Order",
                SecondaryName = "Поръчки",
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Reservation",
                SecondaryName = "Резервации",
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Menu",
                SecondaryName = "Меню и храна",
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Account",
                SecondaryName = "Профил",
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Careers",
                SecondaryName = "Кариера",
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Feedback",
                SecondaryName = "Обратна връзка",
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Other...",
                SecondaryName = "Други...",
            });
        }
    }
}
