namespace Restaurant.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Restaurant.Data.Common;
    using Restaurant.Data.Models;

    internal class CategorySeeder : ISeeder<ApplicationDbContext>
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
                Adjective = "Stunning",
                FontAwesomeIcon = "fa-solid fa-burger",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Beer",
                Adjective = "Craft",
                FontAwesomeIcon = "fa-solid fa-beer-mug-empty",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Pizza",
                Adjective = "Mouth-watering",
                FontAwesomeIcon = "fa-solid fa-pizza-slice",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Desserts",
                Adjective = "Incledible",
                FontAwesomeIcon = "fa-solid fa-cookie",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Sandwiches",
                Adjective = "Scrumptious",
                FontAwesomeIcon = "fa-solid fa-bread-slice",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Chicken",
                Adjective = "Juicy",
                FontAwesomeIcon = "fa-solid fa-drumstick-bite",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Pork",
                Adjective = "Succulent",
                FontAwesomeIcon = "fa-solid fa-bacon",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Hot Dogs",
                Adjective = "Delicious",
                FontAwesomeIcon = "fa-solid fa-hotdog",
            });
        }
    }
}
