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
                IsInSecondaryLanguage = false,
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Reservation",
                IsInSecondaryLanguage = false,
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Menu",
                IsInSecondaryLanguage = false,
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Account",
                IsInSecondaryLanguage = false,
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Careers",
                IsInSecondaryLanguage = false,
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Feedback",
                IsInSecondaryLanguage = false,
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Other...",
                IsInSecondaryLanguage = false,
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Поръчки",
                IsInSecondaryLanguage = true,
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Резервации",
                IsInSecondaryLanguage = true,
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Меню",
                IsInSecondaryLanguage = true,
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Профил",
                IsInSecondaryLanguage = true,
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Кариера",
                IsInSecondaryLanguage = true,
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Обратна връзка",
                IsInSecondaryLanguage = true,
            });

            await dbContext.UserMessageCategories.AddAsync(new UserMessageCategory
            {
                Name = "Други...",
                IsInSecondaryLanguage = true,
            });
        }
    }
}
