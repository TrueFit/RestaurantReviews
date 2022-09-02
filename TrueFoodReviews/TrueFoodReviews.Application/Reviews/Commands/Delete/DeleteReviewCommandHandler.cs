using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Application.Reviews.Common;
using TrueFoodReviews.Domain.Common.Errors;

namespace TrueFoodReviews.Application.Reviews.Commands.Delete;

public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, ErrorOr<DeleteReviewResult>>
{
    private readonly IReviewRepository _reviewRepository;
    
    public DeleteReviewCommandHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }
    
    public async Task<ErrorOr<DeleteReviewResult>> Handle(DeleteReviewCommand command, CancellationToken cancellationToken)
    {
        var review = _reviewRepository.GetReviewById(command.ReviewId);
        if (review == null)
        {
            return Errors.Reviews.ReviewNotFound;
        }
        
        _reviewRepository.Delete(command.ReviewId);
        
        return new DeleteReviewResult(true, "Review deleted");
    }
}