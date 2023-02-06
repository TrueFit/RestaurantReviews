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

        /// <summary>
        /// Sets User attributes
        /// </summary>
        /// <param name="user">Updated User attributes</param>
        public void Update(User user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            IsSuspended = user.IsSuspended;
        }
    }
}
