using ErrorOr;

namespace TrueFoodReviews.Domain.Common.Errors;

public static partial class Errors
{
    public static class Restaurants
    {
        public static Error DuplicateRestaurant => Error.Conflict(
            code: "Restaurants.DuplicateRestaurant",
            description: "A duplicate restaurant already exists.");

        public static Error NotFound => Error.NotFound(
            code: "Restaurants.NotFound",
            description: "The restaurant was not found.");
    }
}