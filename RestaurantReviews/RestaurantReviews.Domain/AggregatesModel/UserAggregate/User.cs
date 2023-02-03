using RestaurantReviews.Domain.Seedwork;

namespace RestaurantReviews.Domain.AggregatesModel.UserAggregate
{
    public class User : Entity, IAggregateRoot
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsSuspended { get; set; }

        public User(string firstName, string lastName, bool isSuspended)
        {
            FirstName = firstName;
            LastName = lastName;
            IsSuspended = isSuspended;
        }

        public static User Create(User user)
        {
            return new User(user.FirstName, user.LastName, user.IsSuspended);
        }

        public void Update(User user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            IsSuspended = user.IsSuspended;
        }
    }
}
