using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Reviews.Common;

namespace TrueFoodReviews.Application.Reviews.Queries.List;

public record ListReviewsByUserIdQuery(Guid UserId) : IRequest<ErrorOr<ListReviewsResult>>;