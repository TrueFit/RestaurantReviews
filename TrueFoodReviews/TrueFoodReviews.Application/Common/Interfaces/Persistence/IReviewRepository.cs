using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Application.Common.Interfaces.Persistence;

public interface IReviewRepository
{
    void Add(Review review);
    void Update(Review review);
    Review? GetReviewById(Guid reviewId);
    List<Review> GetReviewsByRestaurantId(int restaurantId);
    List<Review> GetReviewsByUserId(Guid userId);
    Review? GetReviewByRestaurantIdAndUserId(int requestRestaurantId, Guid requestUserId);
    void Delete(Guid reviewId);
}