using RestaurantReviews.Domain.Seedwork;

namespace RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate
{
    public class Restaurant : Entity, IAggregateRoot
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public Restaurant(int userId, string name, string city)
        {
            UserId = userId;
            Name = name;
            City = city;
            CreatedOn = DateTime.Now;
            LastUpdatedOn = DateTime.Now;
        }

        public void Update(Restaurant restaurant)
        {
            Name = restaurant.Name;
            City = restaurant.City;
            LastUpdatedOn = DateTime.Now;
        }
    }
}
