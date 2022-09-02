using System.ComponentModel.DataAnnotations;

namespace TrueFoodReviews.Contracts.Reviews;

public record AddReviewRequest(
    int RestaurantId,
    Guid UserId,
    string Title,
    string Content,
    [Range(0,10)] int Rating);