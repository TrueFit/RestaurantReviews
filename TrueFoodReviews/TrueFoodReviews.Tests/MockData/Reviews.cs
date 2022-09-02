using TrueFoodReviews.Domain.Entities;
using Guid = System.Guid;

namespace TrueFoodReviews.Tests.MockData;

public static class Reviews
{
    public static List<Review> GetReviews()
    {
        var reviews = new List<Review>
        {
            new()
            {
                Id = new Guid("0C8F8F8F-8F8F-8F8F-8F8F-8F8F8F8F8F8F"),
                Title = "Review 1",
                Content = "This is the first review",
                Rating = 5,
                RestaurantId = 1,
                UserId = new Guid("8F8F8F8F-8F8F-8F8F-8F8F-8F8F8F8F8F8F"),
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Review 2",
                Content = "This is the second review",
                Rating = 4,
                RestaurantId = 1,
                UserId = Guid.NewGuid()
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Review 3",
                Content = "This is the third review",
                Rating = 3,
                RestaurantId = 1,
                UserId = Guid.NewGuid()
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Review 4",
                Content = "This is the fourth review",
                Rating = 2,
                RestaurantId = 2,
                UserId = Guid.NewGuid()
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Review 5",
                Content = "This is the fifth review",
                Rating = 1,
                RestaurantId = 2,
                UserId = Guid.NewGuid()
            }
        };
        
        return reviews;
    }
}