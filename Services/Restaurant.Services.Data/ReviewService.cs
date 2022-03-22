﻿namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;

    public class ReviewService : IReviewService
    {
        private readonly IDeletableEntityRepository<Review> reviewRepository;

        public ReviewService(IDeletableEntityRepository<Review> reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }

        public async Task AddReviewAsync(AddReviewModel model)
        {
            var review = AutoMapperConfig.MapperInstance.Map<Review>(model);
            await this.reviewRepository.AddAsync(review);
            await this.reviewRepository.SaveChangesAsync();
        }

        public T GetById<T>(int id)
        {
            return this.reviewRepository.AllAsNoTrackingWithDeleted().Where(r => r.Id == id).To<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetAllReviews<T>(int itemsPerPage, int page)
        {
            return this.reviewRepository.AllAsNoTracking().OrderByDescending(x => x.CreatedOn).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).To<T>().ToList();
        }

        public IEnumerable<T> GetAllWithDeleted<T>(int itemsPerPage, int page)
        {
            return this.reviewRepository.AllAsNoTrackingWithDeleted().OrderByDescending(x => x.CreatedOn).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).To<T>().ToList();
        }

        public int GetCount()
        {
            return this.reviewRepository.AllAsNoTracking().Count();
        }

        public IEnumerable<T> GetLatestFiveReviews<T>()
        {
            return this.reviewRepository.AllAsNoTracking().OrderByDescending(x => x.CreatedOn).Take(5).To<T>().ToList();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var review = this.reviewRepository.AllAsNoTrackingWithDeleted().FirstOrDefault(r => r.Id == id);

            if (review.IsDeleted)
            {
                throw new InvalidOperationException("This review has already been deleted");
            }

            this.reviewRepository.Delete(review);
            await this.reviewRepository.SaveChangesAsync();
        }
    }
}
