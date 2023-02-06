using RestaurantReviews.Domain.AggregatesModel.ReviewAggregate;
using RestaurantReviews.Domain.Seedwork;

namespace RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate
{
    public interface IRestaurantReviewRepository : IRepository<RestaurantReviewRepository>
    {
        /// <summary>
        /// Validates and adds the review
        /// </summary>
        /// <param name="review">Review to be added</param>
        /// <returns>Identity of added record</returns>
        int AddReview(Review review);

        /// <summary>
        /// Validates and updates the review
        /// </summary>
        /// <param name="id">Identity of Review to be updated</param>
        /// <param name="review">Review attributes to be updated</param>
        /// <returns>Identity of updated record</returns>
        int UpdateReview(int id, Review review);

        /// <summary>
        /// Validates and deletes Review
        /// </summary>
        /// <param name="id">Identity of Review to be deleted</param>
        /// <param name="userId">User requesting delete</param>
        void DeleteReview(int id, int userId);
    }
}
