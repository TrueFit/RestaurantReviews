using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private static List<User> _users = new();
    
    public void Add(User user)
    {
        // TODO: Remove with a real database..
        user.Id = Guid.NewGuid();
        
        _users.Add(user);
    }

    public void Update(User user)
    {
        var userToUpdate = _users.First(u => u.Id == user.Id);
        userToUpdate = user;
    }

    public User? GetUserById(Guid id)
    {
        return _users.SingleOrDefault(u => u.Id == id);
    }
    
    public User? GetUserByUsername(string username)
    {
        return _users.SingleOrDefault(u => u.Username == username);
    }
    
    public void ToggleMute(Guid userId)
    {
        var userToUpdate = _users.First(u => u.Id == userId);
        userToUpdate.IsMuted = !userToUpdate.IsMuted;
    }
}