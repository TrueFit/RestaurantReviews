using RestaurantReviews.Domain.Seedwork;

namespace RestaurantReviews.Domain.AggregatesModel.UserAggregate
{
    public class User : Entity, IAggregateRoot
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsSuspended { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public User(string firstName, string lastName, bool isSuspended)
        {
            FirstName = firstName;
            LastName = lastName;
            IsSuspended = isSuspended;

            // Audit info
            var createdOn = DateTime.Now;
            CreatedOn = createdOn;
            LastUpdatedOn = createdOn;
        }

        public void Update(User user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            IsSuspended = user.IsSuspended;
            LastUpdatedOn = DateTime.Now;
        }
    }
}
