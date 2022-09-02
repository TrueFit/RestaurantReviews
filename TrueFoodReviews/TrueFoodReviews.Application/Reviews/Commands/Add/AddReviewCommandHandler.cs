using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Application.Reviews.Common;
using TrueFoodReviews.Domain.Common.Errors;
using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Application.Reviews.Commands.Add;

public class AddReviewCommandHandler : 
    IRequestHandler<AddReviewCommand, ErrorOr<AddReviewResult>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRestaurantRepository _restaurantRepository;

    public AddReviewCommandHandler(IReviewRepository reviewRepository, IUserRepository userRepository, IRestaurantRepository restaurantRepository)
    {
        _reviewRepository = reviewRepository;
        _userRepository = userRepository;
        _restaurantRepository = restaurantRepository;
    }

    public async Task<ErrorOr<AddReviewResult>> Handle(AddReviewCommand command, CancellationToken cancellationToken)
    {
        // check if user exists and is muted
        var user = _userRepository.GetUserById(command.UserId);

        if (user is null) return Errors.User.NotFound;
        if (user.IsMuted) return Errors.User.UserIsMuted;

            // check if restaurant exists
        if (_restaurantRepository.GetRestaurantById(command.RestaurantId) is null)
        {
            return Errors.Restaurants.NotFound;
        }
        
        // check for review on restaurant by user
        if (_reviewRepository.GetReviewByRestaurantIdAndUserId(command.RestaurantId, command.UserId) is not null)
        {
            return Errors.Reviews.DuplicateReview;
        }

        var review = new Review
        {
            RestaurantId = command.RestaurantId,
            UserId = command.UserId,
            Title = command.Title,
            Content = command.Content,
            Rating = command.Rating,
        };
        
        _reviewRepository.Add(review);
            
        return new AddReviewResult(review);
    }
}