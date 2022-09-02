namespace TrueFoodReviews.Contracts.Restaurants;

public record AddRestaurantRequest(
    string Name,
    string Address,
    string City,
    string State,
    string Description);