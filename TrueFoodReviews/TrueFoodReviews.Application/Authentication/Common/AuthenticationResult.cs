using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Application.Authentication.Common;

public record AuthenticationResult(
    User User,
    string Token);