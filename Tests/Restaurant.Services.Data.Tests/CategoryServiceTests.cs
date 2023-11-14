namespace Restaurant.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;
    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Restaurant.Data.Repositories;
    using Restaurant.Web.ViewModels.Category;
    using Xunit;

    [Collection("Mapper assembly")]
    public class CategoryServiceTests
    {
        private List<Category> categories;
        private Mock<IRestaurantDeletableEntityRepositoryDecorator<Category>> categoryRepository;
        private CategoryService service;

        public CategoryServiceTests()
        {
            this.categories = new List<Category>();
            this.categoryRepository = new Mock<IRestaurantDeletableEntityRepositoryDecorator<Category>>();
            this.service = new CategoryService(this.categoryRepository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnCorrectAmountOfCategories()
        {
            this.categories.Add(new Category { Id = 1, Name = "Test category 1" });
            this.categories.Add(new Category { Id = 2, Name = "Test category 2" });
            this.categories.Add(new Category { Id = 3, Name = "Test category 3" });

            this.categoryRepository.Setup(cr => cr.AllAsNoTracking()).Returns(this.categories.AsQueryable);

            var result = this.service.GetAll<CategoryViewModel>();
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetAllAsKeyValuePairs_ShouldReturnCorrectAmountOfCategories()
        {
            this.categories.Add(new Category { Id = 1, Name = "Test category 1" });
            this.categories.Add(new Category { Id = 2, Name = "Test category 2" });
            this.categories.Add(new Category { Id = 3, Name = "Test category 3" });

            this.categoryRepository.Setup(cr => cr.AllAsNoTracking()).Returns(this.categories.AsQueryable);

            var result = this.service.GetAllAsKeyValuePairs();
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetAllAsKeyValuePairs_ShouldReturnCorrectValues()
        {
            this.categories.Add(new Category { Id = 1, Name = "Test category 1" });

            this.categoryRepository.Setup(cr => cr.AllAsNoTracking()).Returns(this.categories.AsQueryable);

            var result = this.service.GetAllAsKeyValuePairs().FirstOrDefault();
            Assert.Equal("1", result.Key);
            Assert.Equal("Test category 1", result.Value);
        }
    }
}
