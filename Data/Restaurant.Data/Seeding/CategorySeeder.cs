namespace Restaurant.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Restaurant.Data.Models;

    internal class CategorySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Burgers",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Pasta",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Pizza",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Desserts",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Soups",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Chicken",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Pork",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Beef",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Vegetarian",
            });
        }
    }
}
