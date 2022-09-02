using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Application.Reviews.Common;

public record ListReviewsResult(List<Review> Reviews);