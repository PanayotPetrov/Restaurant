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
                SecondaryName = "Бургери",
                SecondaryAdjective = "Зашеметяващи",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Beer",
                Adjective = "Craft",
                FontAwesomeIcon = "fa-solid fa-beer-mug-empty",
                SecondaryName = "Бира",
                SecondaryAdjective = "Крафт",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Pizza",
                Adjective = "Mouth-watering",
                FontAwesomeIcon = "fa-solid fa-pizza-slice",
                SecondaryName = "Пица",
                SecondaryAdjective = "Апетитна",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Desserts",
                Adjective = "Incledible",
                FontAwesomeIcon = "fa-solid fa-cookie",
                SecondaryName = "Десерти",
                SecondaryAdjective = "Невероятни",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Sandwiches",
                Adjective = "Scrumptious",
                FontAwesomeIcon = "fa-solid fa-bread-slice",
                SecondaryName = "Сандвичи",
                SecondaryAdjective = "Съблазнителни",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Chicken",
                Adjective = "Juicy",
                FontAwesomeIcon = "fa-solid fa-drumstick-bite",
                SecondaryName = "Сочно",
                SecondaryAdjective = "Пилешко",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Pork",
                Adjective = "Succulent",
                FontAwesomeIcon = "fa-solid fa-bacon",
                SecondaryName = "Свинско",
                SecondaryAdjective = "Ароматно",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Hot Dogs",
                Adjective = "Delicious",
                FontAwesomeIcon = "fa-solid fa-hotdog",
                SecondaryName = "Вкусни",
                SecondaryAdjective = "Хотдози",
            });
        }
    }
}
