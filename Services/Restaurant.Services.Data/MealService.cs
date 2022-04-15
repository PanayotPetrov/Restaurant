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

        public async Task<int> CreateAsync(AddMealModel model, string imagePath)
        {
            var meal = AutoMapperConfig.MapperInstance.Map<Meal>(model);
            await this.AddImageAsync(model, imagePath, meal);
            await this.mealRepository.AddAsync(meal);
            await this.mealRepository.SaveChangesAsync();
            return meal.Id;
        }

        public IEnumerable<T> GetAllMeals<T>()
        {
            return this.mealRepository.AllAsNoTracking().Include(i => i.Category).To<T>().ToList();
        }

        public IEnumerable<T> GetAllWithPagination<T>(int itemsPerPage, int page)
        {
            return this.mealRepository.AllAsNoTrackingWithDeleted().OrderByDescending(x => x.CreatedOn).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).To<T>().ToList();
        }

        public int GetMealCount()
        {
            return this.mealRepository.AllAsNoTrackingWithDeleted().Count();
        }

        public T GetByIdWithDeleted<T>(int id)
        {
            return this.mealRepository.AllAsNoTrackingWithDeleted().Where(m => m.Id == id).To<T>().FirstOrDefault();
        }

        public bool IsMealIdValid(int mealId)
        {
            return this.mealRepository.AllAsNoTrackingWithDeleted().Any(m => m.Id == mealId);
        }

        public async Task<bool> DeleteByIdAsync(int mealId)
        {
            var meal = this.mealRepository.AllWithDeleted().FirstOrDefault(m => m.Id == mealId);

            if (meal is null)
            {
                throw new NullReferenceException();
            }

            if (meal.IsDeleted)
            {
                return false;
            }

            this.mealRepository.Delete(meal);
            await this.mealRepository.SaveChangesAsync();
            return true;
        }

        public async Task UpdateAsync(EditMealModel model, string imagePath)
        {
            var meal = this.mealRepository.AllWithDeleted().Include(m => m.Image).FirstOrDefault(m => m.Id == model.Id);

            if (meal is null)
            {
                throw new NullReferenceException();
            }

            meal.Name = model.Name;
            meal.Description = model.Description;
            meal.CategoryId = model.CategoryId;
            meal.Price = model.Price;
            if (model.Image is not null)
            {
                var physicalPath = $"{imagePath}/meals/{meal.Image.Id}.{meal.Image.Extension}";
                File.Delete(physicalPath);
                this.mealImageRepository.Delete(meal.Image);
                await this.AddImageAsync(model, imagePath, meal);
            }

            await this.mealRepository.SaveChangesAsync();
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var meal = this.mealRepository.AllWithDeleted().FirstOrDefault(r => r.Id == id);

            if (meal is null)
            {
                throw new NullReferenceException();
            }

            if (!meal.IsDeleted)
            {
                return false;
            }

            meal.IsDeleted = false;
            meal.DeletedOn = null;
            await this.mealRepository.SaveChangesAsync();
            return true;
        }

        private async Task AddImageAsync(AddMealModel model, string imagePath, Meal meal)
        {
            Directory.CreateDirectory($"{imagePath}/meals/");
            var extension = Path.GetExtension(model.Image.FileName).TrimStart('.');

            if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
            {
                throw new InvalidOperationException($"Invalid image extension {extension}");
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
