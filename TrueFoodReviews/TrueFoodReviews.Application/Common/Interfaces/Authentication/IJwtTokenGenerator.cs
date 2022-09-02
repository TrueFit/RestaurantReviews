using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}