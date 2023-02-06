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

        /// <summary>
        /// Validates associated Restaurant and User
        /// </summary>
        /// <param name="review">Review to be validated</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Validate(Review review)
        {
            if (_restaurantRepository.GetById(review.RestaurantId) is null)
                throw new ArgumentOutOfRangeException("Restaurant no valid");
            ValidateUser(review.UserId);
        }

        /// <summary>
        /// Validates associated User
        /// </summary>
        /// <param name="userId">Identity of User to be validated</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private void ValidateUser(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user is null)
                throw new ArgumentOutOfRangeException("Not a valid user idenity.");
            if (user.IsSuspended)
                throw new ArgumentException("User suspended and not allowed to contribute.");
        }
    }
}
