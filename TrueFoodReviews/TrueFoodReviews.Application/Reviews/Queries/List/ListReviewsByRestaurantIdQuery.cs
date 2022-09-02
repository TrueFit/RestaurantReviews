using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Reviews.Common;

namespace TrueFoodReviews.Application.Reviews.Queries.List;

public record ListReviewsByRestaurantIdQuery(int RestaurantId) : IRequest<ErrorOr<ListReviewsResult>>;