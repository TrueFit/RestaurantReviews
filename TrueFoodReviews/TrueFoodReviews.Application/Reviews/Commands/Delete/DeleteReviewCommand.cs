using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Reviews.Common;

namespace TrueFoodReviews.Application.Reviews.Commands.Delete;

public record DeleteReviewCommand(Guid ReviewId) : IRequest<ErrorOr<DeleteReviewResult>>;