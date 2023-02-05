using RestaurantReviews.Domain.AggregatesModel.ReviewAggregate;
using RestaurantReviews.Domain.Seedwork;

namespace RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate
{
    public interface IRestaurantReviewRepository : IRepository<RestaurantReviewRepository>
    {
        int AddReview(Review review);
        int UpdateReview(int id, Review review);
        void DeleteReview(int id, int userId);
    }
}
