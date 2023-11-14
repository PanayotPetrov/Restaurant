namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Restaurant.Data.Models;
    using Restaurant.Data.Repositories;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;

    public class ReviewService : PaginationService<Review>, IReviewService
    {
        private readonly IRestaurantDeletableEntityRepositoryDecorator<Review> reviewRepository;

        public ReviewService(IRestaurantDeletableEntityRepositoryDecorator<Review> reviewRepository)
            : base(reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }

        public async Task AddReviewAsync(AddReviewModel model)
        {
            var review = AutoMapperConfig.MapperInstance.Map<Review>(model);
            await this.reviewRepository.AddAsync(review);
            await this.reviewRepository.SaveChangesAsync();
        }

        public T GetById<T>(int id, bool getDeleted = false)
        {
            var review = getDeleted
                ? this.reviewRepository.AllAsNoTrackingWithDeleted().Where(x => x.Id == id).To<T>().FirstOrDefault()
                : this.reviewRepository.AllAsNoTracking().Where(x => x.Id == id).To<T>().FirstOrDefault();

            return review;
        }

        public IEnumerable<T> GetLatestFiveReviews<T>()
        {
            return this.reviewRepository.AllAsNoTracking().OrderByDescending(x => x.CreatedOn).Take(5).To<T>().ToList();
        }

        public bool IsReviewIdValid(int reviewId)
        {
            return this.reviewRepository.AllAsNoTrackingWithDeleted().Any(r => r.Id == reviewId);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var review = this.reviewRepository.AllWithDeleted().FirstOrDefault(r => r.Id == id);

            if (review is null)
            {
                throw new NullReferenceException();
            }

            if (review.IsDeleted)
            {
                return false;
            }

            this.reviewRepository.Delete(review);
            await this.reviewRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var review = this.reviewRepository.AllWithDeleted().FirstOrDefault(r => r.Id == id);

            if (review is null)
            {
                throw new NullReferenceException();
            }

            if (!review.IsDeleted)
            {
                return false;
            }

            review.IsDeleted = false;
            review.DeletedOn = null;
            await this.reviewRepository.SaveChangesAsync();
            return true;
        }
    }
}
