using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Application.Restaurants.Common;

public record ListRestaurantsResult(
    List<Restaurant> Restaurants);