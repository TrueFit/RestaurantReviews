using RestaurantReviews.Domain.AggregatesModel.ReviewAggregate;
using RestaurantReviews.Domain.AggregatesModel.UserAggregate;
using RestaurantReviews.Domain.Seedwork;

namespace RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate
{
    public class RestaurantReviewRepository : IRestaurantReviewRepository, IAggregateRoot
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;

        public RestaurantReviewRepository(IRestaurantRepository restaurantRepo, IReviewRepository reviewRepo, IUserRepository userRepo)
        {
            _restaurantRepository= restaurantRepo;
            _reviewRepository= reviewRepo;
            _userRepository= userRepo;
        }

        public int AddReview(Review review)
        {
            Validate(review);
            return _reviewRepository.Insert(review);
        }

        public int UpdateReview(int id, Review review)
        {
            Validate(review);
            return _reviewRepository.Update(id, review);
        }

        public void DeleteReview(int id, int userId)
        {
            ValidateUser(userId);
            _reviewRepository.Delete(id);
        }

        private void Validate(Review review)
        {
            if (_restaurantRepository.GetById(review.RestaurantId) == null)
                throw new Exception("Restaurant no valid");
            ValidateUser(review.UserId);
        }

        private void ValidateUser(int userId)
        {
            if (_userRepository.GetById(userId).IsSuspended)
                throw new Exception("User not allowed to contribute.");
        }
    }
}
