namespace TrueFoodReviews.Contracts.Reviews;

public record AddReviewRequest(
    int RestaurantId,
    Guid UserId,
    string Title,
    string Content,
    int Rating);