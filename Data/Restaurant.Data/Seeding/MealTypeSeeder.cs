namespace Restaurant.Data.Seeding
{
    using Restaurant.Data.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    internal class MealTypeSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.MealTypes.Any())
            {
                return;
            }

            await dbContext.MealTypes.AddAsync(new MealType
            {
                Name = "Breakfast",
            });
            await dbContext.MealTypes.AddAsync(new MealType
            {
                Name = "Lunch",
            }); 
            await dbContext.MealTypes.AddAsync(new MealType
            {
                Name = "Dinner",
            });
        }
    }
}