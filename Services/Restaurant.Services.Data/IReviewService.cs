namespace Restaurant.Services.Data
{
    using System.Collections.Generic;

    using Restaurant.Web.ViewModels.InputModels;

    public interface IReviewService
    {
        IEnumerable<T> GetAllReviews<T>();

        bool AddReview(AddReviewInputModel model);

        IEnumerable<T> GetLatestFiveReviews<T>();
    }
}
