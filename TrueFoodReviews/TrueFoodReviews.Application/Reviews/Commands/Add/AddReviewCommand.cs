using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Reviews.Common;

namespace TrueFoodReviews.Application.Reviews.Commands.Add;

public record AddReviewCommand(
    int RestaurantId,
    Guid UserId,
    string Title,
    string Content,
    int Rating) : IRequest<ErrorOr<AddReviewResult>>;