using RestaurantReviews.Domain.Seedwork;

namespace RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate
{
    public class Restaurant : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string City { get; set; }

        public Restaurant(string name, string city)
        {
            Name = name;
            City = city;
        }

        /// <summary>
        /// Sets Restaurant attributes
        /// </summary>
        /// <param name="restaurant">Updated Restaurant attributes</param>
        public void Update(Restaurant restaurant)
        {
            Name = restaurant.Name;
            City = restaurant.City;
        }
    }
}
