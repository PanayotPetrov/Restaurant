namespace Restaurant.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http.Internal;
    using Moq;
    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels.Meal;
    using Xunit;

    [Collection("Mapper assembly")]
    public class MealServiceTests
    {
        private List<Meal> meals;
        private Mock<IDeletableEntityRepository<Meal>> repository;
        private Mock<IRepository<MealImage>> mealImageRepository;

        private MealService service;

        public MealServiceTests()
        {
            this.meals = new List<Meal>();
            this.repository = new Mock<IDeletableEntityRepository<Meal>>();
            this.mealImageRepository = new Mock<IRepository<MealImage>>();

            this.service = new MealService(this.repository.Object, this.mealImageRepository.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnMealIdOnCreation()
        {
            var model = new AddMealModel
            {
                Description = "Test description",
                CategoryId = 1,
                Name = "Test meal name",
                Price = 10M,
            };
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("dummy image"));
            model.Image = new FormFile(stream, 0, 0, "Data", "image.png");

            this.repository.Setup(r => r.AddAsync(It.IsAny<Meal>())).Callback((Meal meal) => this.meals.Add(meal));

            await this.service.CreateAsync(model, "my test path");
            Assert.Single(this.meals);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_IfInvalidImgExtentionSelected()
        {
            var model = new AddMealModel
            {
                Description = "Test description",
                CategoryId = 1,
                Name = "Test meal name",
                Price = 10M,
            };
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("dummy image"));
            model.Image = new FormFile(stream, 0, 0, "Data", "image.test");

            this.repository.Setup(r => r.AddAsync(It.IsAny<Meal>())).Callback((Meal meal) => this.meals.Add(meal));

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await this.service.CreateAsync(model, "my test path"));
        }

        [Fact]
        public async Task UpdateAsyncc_ShouldAmendMealDetails()
        {
            this.GenerateMeals(1);

            var model = new EditMealModel
            {
                Id = 1,
                Description = "Test description",
                CategoryId = 1,
                Name = "New test name",
                Price = 10M,
            };

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("dummy image"));
            model.Image = new FormFile(stream, 0, 0, "Data", "image.png");

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.meals.AsQueryable);

            await this.service.UpdateAsync(model, "my test path");
            Assert.Equal("New test name", this.meals.FirstOrDefault().Name);
        }

        [Fact]
        public async Task UpdateAsyncc_ShouldThrowException_IfInvalidImgExtentionSelected()
        {
            this.GenerateMeals(1);

            var model = new EditMealModel
            {
                Id = 1,
                Description = "Test description",
                CategoryId = 1,
                Name = "Test meal name",
                Price = 10M,
            };
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("dummy image"));
            model.Image = new FormFile(stream, 0, 0, "Data", "image.test");

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.meals.AsQueryable);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await this.service.UpdateAsync(model, "my test path"));
        }

        [Fact]
        public void GetAll_ShouldReturnAllMeals()
        {
            this.GenerateMeals(3);

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.meals.AsQueryable());
            var result = this.service.GetAllMeals<MealViewModel>();
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetAllWithPagination_ShouldReturnCorrectValue()
        {
            this.GenerateMeals(5);

            this.repository.Setup(x => x.AllAsNoTrackingWithDeleted()).Returns(this.meals.AsQueryable());

            var result = this.service.GetAllWithPagination<MealViewModel>(3, 1, true);
            var result2 = this.service.GetAllWithPagination<MealViewModel>(3, 2, true);

            Assert.Equal(3, result.Count());
            Assert.Equal(2, result2.Count());
        }

        [Fact]
        public void GetCount_ShouldReturnCorrectValue()
        {
            this.GenerateMeals(5);

            this.repository.Setup(x => x.AllAsNoTrackingWithDeleted()).Returns(this.meals.AsQueryable());

            var result = this.service.GetCount(true);

            Assert.Equal(5, result);
        }

        [Fact]
        public void GetByIdWithDeleted_ShouldReturnCorrectMeal()
        {
            this.GenerateMeals(5);

            this.repository.Setup(x => x.AllAsNoTrackingWithDeleted()).Returns(this.meals.AsQueryable());

            var result = this.service.GetById<AdminMealViewModel>(1, true);

            Assert.Equal(1, result.Id);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(6)]

        public void IsMealIdValid_ShouldReturnCorrectValue(int mealId)
        {
            this.GenerateMeals(5);

            this.repository.Setup(x => x.AllAsNoTrackingWithDeleted()).Returns(this.meals.AsQueryable());

            var result = this.service.IsMealIdValid(mealId);

            if (mealId == 1)
            {
                Assert.True(result);
            }
            else
            {
                Assert.False(result);
            }
        }

        [Fact]
        public async Task RestoreAsync_ShouldReturnFalseIfMealIsNotDeleted()
        {
            this.meals.Add(new Meal
            {
                Id = 1,
                IsDeleted = false,
            });

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.meals.AsQueryable());
            Assert.False(await this.service.RestoreAsync(1));
        }

        [Fact]
        public async Task RestoreAsync_ShouldReturnTrueIfMealIsRestored()
        {
            this.meals.Add(new Meal
            {
                Id = 1,
                IsDeleted = true,
            });

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.meals.AsQueryable());
            Assert.True(await this.service.RestoreAsync(1));
        }

        [Fact]
        public async Task RestoreAsync_ShouldRestoreMeal()
        {
            var meal = new Meal
            {
                Id = 1,
                IsDeleted = true,
            };

            this.meals.Add(meal);

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.meals.AsQueryable());
            await this.service.RestoreAsync(meal.Id);
            Assert.False(this.meals.FirstOrDefault().IsDeleted);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldReturnTrueIfSuccessfullyDeleted()
        {
            this.meals.Add(new Meal
            {
                Id = 1,
                IsDeleted = false,
            });

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.meals.AsQueryable());
            Assert.True(await this.service.DeleteByIdAsync(1));
        }

        [Fact]
        public async Task DeleteById_ShouldDeleteMeal()
        {
            var meal = new Meal
            {
                Id = 1,
                IsDeleted = false,
            };
            this.meals.Add(meal);

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.meals.AsQueryable());
            this.repository.Setup(r => r.Delete(It.IsAny<Meal>())).Callback((Meal meal) => this.meals.Remove(meal));
            await this.service.DeleteByIdAsync(meal.Id);
            Assert.Empty(this.meals);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldReturnFalseIfMealAlreadyDeleted()
        {
            this.meals.Add(new Meal
            {
                Id = 1,
                IsDeleted = true,
            });

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.meals.AsQueryable());
            Assert.False(await this.service.DeleteByIdAsync(1));
        }

        private void GenerateMeals(int numberOfMeals)
        {
            for (int i = 1; i <= numberOfMeals; i++)
            {
                this.meals.Add(new Meal
                {
                    Id = i,
                    CategoryId = i,
                    Name = $"Test meal {i}",
                    Price = i,
                    Description = "Test description",
                    Category = new Category { Id = i, },
                    Image = new MealImage { Id = "test id", Extension = "test extention" },
                });
            }
        }
    }
}
