using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Application.Reviews.Common;

namespace TrueFoodReviews.Application.Reviews.Queries.List;

public class ListReviewsByUserIdQueryHandler : IRequestHandler<ListReviewsByUserIdQuery, ErrorOr<ListReviewsResult>>
{
    private readonly IReviewRepository _reviewRepository;

    public ListReviewsByUserIdQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<ErrorOr<ListReviewsResult>> Handle(ListReviewsByUserIdQuery query, CancellationToken cancellationToken)
    {
        var reviews = _reviewRepository.GetReviewsByUserId(query.UserId);

        var result = new ListReviewsResult(reviews);
        
        return result;
    }
}