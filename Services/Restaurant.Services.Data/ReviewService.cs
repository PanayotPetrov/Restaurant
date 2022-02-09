namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        public bool AddReview(AddReviewInputModel model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAllReviews<T>()
        {
            return this.reviewRepository.All().To<T>().ToList();
        }

        public IEnumerable<T> GetLatestFiveReviews<T>()
        {
            return this.reviewRepository.All().Include(r => r.ApplicationUser).OrderByDescending(x => x.CreatedOn).Take(5).To<T>().ToList();
        }
    }
}
