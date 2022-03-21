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
    using Restaurant.Services.Models;

    public class MealService : IMealService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };
        private readonly IDeletableEntityRepository<Meal> mealRepository;
        private readonly IRepository<MealImage> mealImageRepository;

        public MealService(IDeletableEntityRepository<Meal> mealRepository, IRepository<MealImage> mealImageRepository)
        {
            this.mealRepository = mealRepository;
            this.mealImageRepository = mealImageRepository;
        }

        public async Task CreateAsync(AddMealModel model, string imagePath)
        {
            var meal = AutoMapperConfig.MapperInstance.Map<Meal>(model);
            await this.AddImage(model, imagePath, meal);
            await this.mealRepository.AddAsync(meal);
            await this.mealRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllMeals<T>()
        {
            return this.mealRepository.All().Include(i => i.Category).To<T>().ToList();
        }

        public IEnumerable<T> GetAllWithPagination<T>(int itemsPerPage, int page)
        {
            return this.mealRepository.All().OrderByDescending(x => x.CreatedOn).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).To<T>().ToList();
        }

        public T GetById<T>(int mealId)
        {
            return this.mealRepository.AllAsNoTracking().Where(m => m.Id == mealId).To<T>().FirstOrDefault();
        }

        public int GetMealCount()
        {
            return this.mealRepository.AllAsNoTracking().Count();
        }

        public async Task DeleteById(int mealId)
        {
            var meal = this.mealRepository.All().FirstOrDefault(m => m.Id == mealId);
            this.mealRepository.Delete(meal);
            await this.mealRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(AddMealModel model, int mealId, string imagePath)
        {
            var meal = this.mealRepository.All().Include(m => m.Image).FirstOrDefault(m => m.Id == mealId);
            meal.Name = model.Name;
            meal.Description = model.Description;
            meal.CategoryId = model.CategoryId;
            meal.Price = model.Price;
            if (model.Image is not null)
            {
                var physicalPath = $"{imagePath}/meals/{meal.Image.Id}.{meal.Image.Extension}";
                File.Delete(physicalPath);
                this.mealImageRepository.Delete(meal.Image);
                await this.AddImage(model, imagePath, meal);
            }

            await this.mealRepository.SaveChangesAsync();
        }

        private async Task AddImage(AddMealModel model, string imagePath, Meal meal)
        {
            Directory.CreateDirectory($"{imagePath}/meals/");
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

            var physicalPath = $"{imagePath}/meals/{dbImage.Id}.{extension}";
            using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await model.Image.CopyToAsync(fileStream);
        }
    }
}
