using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    void Add(User user);
    User? GetUserById(Guid id);
    User? GetUserByUsername(string username);
    void ToggleMute(Guid userId);
}