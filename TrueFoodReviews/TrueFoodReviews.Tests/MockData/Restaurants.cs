using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Tests.MockData;

public static class Restaurants
{
    public static List<Restaurant> GetRestaurants()
    {
        return new List<Restaurant>
        {
            new Restaurant
            {
                Id = 1,
                Name = "Restaurant 1",
                Address = "Address 1",
                City = "City 1",
                State = "State 1"
            },
            new Restaurant
            {
                Id = 2,
                Name = "Restaurant 2",
                Address = "Address 2",
                City = "City 2",
                State = "State 2"
            },
            new Restaurant
            {
                Id = 3,
                Name = "Restaurant 3",
                Address = "Address 3",
                City = "City 3",
                State = "State 3"
            }
        };
    }
}