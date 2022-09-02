namespace TrueFoodReviews.Domain.Entities;

public class Review
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int Rating { get; set; }
    
    public int RestaurantId { get; set; }
    public Guid UserId { get; set; }
}