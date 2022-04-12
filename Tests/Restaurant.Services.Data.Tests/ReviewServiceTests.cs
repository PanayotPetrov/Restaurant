namespace Restaurant.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels.Review;
    using Xunit;

    [Collection("Mapper assembly")]
    public class ReviewServiceTests
    {
        private List<Review> reviews;
        private Mock<IDeletableEntityRepository<Review>> repository;
        private ReviewService service;

        public ReviewServiceTests()
        {
            this.reviews = new List<Review>();
            this.repository = new Mock<IDeletableEntityRepository<Review>>();
            this.service = new ReviewService(this.repository.Object);
        }

        [Fact]
        public void IsReviewIdValid_ShouldReturnTrueIfReviewExists()
        {
            this.GenerateReviews(3);
            this.repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(this.reviews.AsQueryable());

            Assert.True(this.service.IsReviewIdValid(1));
            this.repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public void IsReviewIdValid_ShouldReturnFalseIfIdIsInvalid()
        {
            this.repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(this.reviews.AsQueryable());

            Assert.False(this.service.IsReviewIdValid(1));
            this.repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public void GetCount_ShouldReturnCorrectNumber()
        {
            this.GenerateReviews(3);
            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.reviews.AsQueryable());

            Assert.Equal(3, this.service.GetCount());
            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetCountWithDeleted_ShouldReturnCorrectNumber()
        {
            this.GenerateReviews(3);
            this.repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(this.reviews.AsQueryable());

            Assert.Equal(3, this.service.GetCountWithDeleted());
            this.repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectEntity()
        {
            this.GenerateReviews(2);
            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.reviews.AsQueryable());
            var result = this.service.GetById<AdminReviewViewModel>(1);
            Assert.Equal(1, result.Id);
            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetByIdWithDeleted_ShouldReturnCorrectEntity()
        {
            this.GenerateReviews(2);
            this.repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(this.reviews.AsQueryable());
            var result = this.service.GetByIdWithDeleted<AdminReviewViewModel>(1);
            Assert.Equal(1, result.Id);
            this.repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public async Task DeleteById_ShouldReturnFalse_IfReviewIsAlreadyDeleted()
        {
            var review = new Review
            {
                Id = 1,
                IsDeleted = true,
            };
            this.reviews.Add(review);

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.reviews.AsQueryable());
            Assert.False(await this.service.DeleteByIdAsync(1));
            this.repository.Verify(x => x.AllWithDeleted(), Times.Once);
        }

        [Fact]
        public async Task DeleteById_ShouldReturnTrueIfHasNotMarkedAsDeleted()
        {
            var review = new Review
            {
                Id = 1,
                IsDeleted = false,
            };
            this.reviews.Add(review);

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.reviews.AsQueryable());
            Assert.True(await this.service.DeleteByIdAsync(1));
            this.repository.Verify(x => x.AllWithDeleted(), Times.Once);
        }

        [Fact]
        public async Task DeleteById_ShouldDeleteReview()
        {
            var review = new Review
            {
                Id = 1,
                IsDeleted = false,
            };
            this.reviews.Add(review);

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.reviews.AsQueryable());
            this.repository.Setup(r => r.Delete(It.IsAny<Review>())).Callback((Review review) => this.reviews.Remove(review));
            await this.service.DeleteByIdAsync(1);
            Assert.Empty(this.reviews);
        }

        [Fact]
        public async Task RestoreAsync_ShouldReturnTrue_IfReviewIsMarkedAsDeleted()
        {
            var review = new Review
            {
                Id = 1,
                IsDeleted = true,
            };
            this.reviews.Add(review);

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.reviews.AsQueryable());
            Assert.True(await this.service.RestoreAsync(1));
            this.repository.Verify(x => x.AllWithDeleted(), Times.Once);
        }

        [Fact]
        public async Task RestoreAsync_ShouldRestoreReview()
        {
            var review = new Review
            {
                Id = 1,
                IsDeleted = true,
            };
            this.reviews.Add(review);

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.reviews.AsQueryable());
            await this.service.RestoreAsync(1);
            Assert.False(this.reviews.FirstOrDefault().IsDeleted);
            this.repository.Verify(x => x.AllWithDeleted(), Times.Once);
        }

        [Fact]
        public async Task RestoreAsync_ShouldReturnFalse_IfReviewHasNotBeenDeletedInitially()
        {
            var review = new Review
            {
                Id = 1,
                IsDeleted = false,
            };
            this.reviews.Add(review);

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.reviews.AsQueryable());
            Assert.False(await this.service.RestoreAsync(1));
            this.repository.Verify(x => x.AllWithDeleted(), Times.Once);
        }

        [Fact]
        public void GetFiveLatestReviews_ShouldReturnFiveEntities()
        {
            this.GenerateReviews(7);
            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.reviews.AsQueryable());
            var result = this.service.GetLatestFiveReviews<ReviewViewModel>();
            Assert.Equal(5, result.Count());
            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task AddReviewAsync_ShouldAddReview()
        {
            var addreviewModel = new AddReviewModel
            {
                ApplicationUserId = "Test id",
                Description = "Test description",
                Rating = "5",
                Summary = "Test summary",
            };

            this.repository.Setup(r => r.AddAsync(It.IsAny<Review>()))
                .Callback((Review review) => this.reviews.Add(review));
            await this.service.AddReviewAsync(addreviewModel);
            Assert.Single(this.reviews);
        }

        [Fact]
        public void GetAllReviews_ShouldReturnCorrectAmountOfEntities()
        {
            this.GenerateReviews(9);
            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.reviews.AsQueryable());
            var firstPage = this.service.GetAllReviews<ReviewViewModel>(5, 1);
            var secondPage = this.service.GetAllReviews<ReviewViewModel>(5, 2);
            Assert.Equal(5, firstPage.Count());
            Assert.Equal(4, secondPage.Count());
        }

        [Fact]
        public void GetAllReviewsWithDeleted_ShouldReturnCorrectAmountOfEntities()
        {
            this.GenerateReviews(9);
            this.repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(this.reviews.AsQueryable());
            var firstPage = this.service.GetAllWithDeleted<ReviewViewModel>(5, 1);
            var secondPage = this.service.GetAllWithDeleted<ReviewViewModel>(5, 2);
            Assert.Equal(5, firstPage.Count());
            Assert.Equal(4, secondPage.Count());
        }

        private void GenerateReviews(int numberOfReviews)
        {
            for (int i = 1; i <= numberOfReviews; i++)
            {
                this.reviews.Add(new Review
                {
                    Id = 1,
                    Description = "Test description",
                    Summary = "Test summary",
                    Rating = 5,
                    CreatedOn = DateTime.Parse($"0{i}/04/2022"),
                    DeletedOn = null,
                    ModifiedOn = null,
                    IsDeleted = false,
                    ApplicationUser = new ApplicationUser
                    {
                        FirstName = "Test FirstName",
                        LastName = "Test LastName",
                        UserImage = new UserImage
                        {
                            Id = "Test user image id",
                            Extension = ".png",
                        },
                    },
                });
            }
        }
    }
}
