using RestaurantReviews.Domain.Seedwork;

namespace RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate
{
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        Restaurant GetById(int id);
        int Insert(Restaurant restaurant);
        int Update(int id, Restaurant restaurant);
        IEnumerable<Restaurant> Get(string city);
    }
}
