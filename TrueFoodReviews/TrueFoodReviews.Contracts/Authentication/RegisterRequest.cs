namespace TrueFoodReviews.Contracts.Authentication;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Username,
    string Password);