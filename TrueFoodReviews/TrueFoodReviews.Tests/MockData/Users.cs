using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Tests.MockData;

public static class Users
{
    public static List<User> GetUsers()
    {
        return new List<User>
        {
            new User
            {
                Id = new Guid("0F8F8F8F-8F8F-8F8F-8F8F-8F8F8F8F8F8F"),
                FirstName = "John",
                LastName = "Doe",
                Username = "User1",
                Password = "Password1",
                Email = "user1@users.com"
            }
        };
    }
}