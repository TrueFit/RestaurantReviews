using ErrorOr;

namespace TrueFoodReviews.Domain.Common.Errors;

public static partial class Errors
{
    public static class Reviews
    {
        public static Error DuplicateReview => Error.Conflict(
            code: "Reviews.DuplicateReview",
            description: "You already have a review for this restaurant.");

        public static Error ReviewNotFound => Error.NotFound(
            code: "Reviews.ReviewNotFound",
            description: "Review not found.");
    }
}