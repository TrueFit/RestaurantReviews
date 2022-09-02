using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Application.Reviews.Common;

namespace TrueFoodReviews.Application.Reviews.Queries.List;

public class ListReviewsByRestaurantIdQueryHandler : IRequestHandler<ListReviewsByRestaurantIdQuery, ErrorOr<ListReviewsResult>>
{
    private readonly IReviewRepository _reviewRepository;
    
    public ListReviewsByRestaurantIdQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }
    
    public async Task<ErrorOr<ListReviewsResult>> Handle(ListReviewsByRestaurantIdQuery query, CancellationToken cancellationToken)
    {
        var reviews = _reviewRepository.GetReviewsByRestaurantId(query.RestaurantId);

        var result = new ListReviewsResult(reviews);
        
        return result;
    }
}