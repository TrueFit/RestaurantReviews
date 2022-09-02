using ErrorOr;

namespace TrueFoodReviews.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateUsername => Error.Conflict(
            code: "User.DuplicateUsername",
            description: "Username is already in use.");

        public static Error NotFound => Error.NotFound(
            code: "User.UserNotFound",
            description: "User not found.");

        public static Error UserIsMuted => Error.Conflict(
            code: "User.UserIsMuted",
            description: "User is muted.");
    }
}