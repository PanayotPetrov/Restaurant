namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Web.ViewModels.InputModels;

    public class MealService : IMealService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };
        private readonly IDeletableEntityRepository<Meal> mealRepository;

        public MealService(IDeletableEntityRepository<Meal> mealRepository)
        {
            this.mealRepository = mealRepository;
        }

        public async Task CreateAsync(AddMealInputModel model, string imagePath)
        {
            var meal = new Meal
            {
                CategoryId = model.CategoryId,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
            };

            Directory.CreateDirectory($"{imagePath}/recipes/");
            var extension = Path.GetExtension(model.Image.FileName).TrimStart('.');

            if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
            {
                throw new Exception($"Invalid image extension {extension}");
            }

            var dbImage = new MealImage
            {
                Extension = extension,
            };

            meal.Image = dbImage;

            var physicalPath = $"{imagePath}/recipes/{dbImage.Id}.{extension}";
            using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await model.Image.CopyToAsync(fileStream);
            await this.mealRepository.AddAsync(meal);
            await this.mealRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllMeals<T>()
        {
            return this.mealRepository.All().Include(i => i.Category).To<T>().ToList();
        }

        public int GetMealCount()
        {
            return this.mealRepository.AllAsNoTracking().Count();
        }
    }
}
