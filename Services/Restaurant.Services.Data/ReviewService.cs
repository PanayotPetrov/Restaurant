namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Web.ViewModels.InputModels;

    public class ReviewService : IReviewService
    {
        private readonly IDeletableEntityRepository<Review> reviewRepository;

        public ReviewService(IDeletableEntityRepository<Review> reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }

        public async Task AddReviewAsync(AddReviewInputModel model)
        {
            var review = new Review
            {
                ApplicationUserId = model.ApplicationUserId,
                Description = model.Description,
                Rating = model.Rating,
            };

            await this.reviewRepository.AddAsync(review);
            await this.reviewRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllReviews<T>()
        {
            return this.reviewRepository.All().OrderByDescending(x => x.CreatedOn).To<T>().ToList();
        }

        public IEnumerable<T> GetLatestFiveReviews<T>()
        {
            return this.reviewRepository.All().Include(r => r.ApplicationUser).OrderByDescending(x => x.CreatedOn).Take(5).To<T>().ToList();
        }
    }
}
