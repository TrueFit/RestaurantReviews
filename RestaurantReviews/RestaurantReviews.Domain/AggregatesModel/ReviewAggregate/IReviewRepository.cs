using RestaurantReviews.Domain.Seedwork;

namespace RestaurantReviews.Domain.AggregatesModel.ReviewAggregate
{
    public interface IReviewRepository : IRepository<Review>
    {
        Review GetById(int id);
        int Insert(Review review);
        int Update(int id, Review review);
        void Delete(int id);
        IEnumerable<Review> Get(int? restaurantId = null, int? userId = null);
    }
}
