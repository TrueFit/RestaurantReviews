using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Infrastructure.Persistence;

public class RestaurantRepository : IRestaurantRepository
{
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private static List<Restaurant> _restaurants = new();
    
    public void Add(Restaurant restaurant)
    {
        // TODO: Replace with real db implementation
        restaurant.Id = _restaurants.Count;
        
        _restaurants.Add(restaurant);
    }
    
    public List<Restaurant> GetAll()
    {
        return _restaurants;
    }
    
    public List<Restaurant> GetRestaurantByName(string name)
    {
        return _restaurants.Where(r => r.Name.Contains(name)).ToList();
    }
    
    public List<Restaurant> GetRestaurantByCity(string city)
    {
        return _restaurants.Where(r => r.City.Contains(city)).ToList();
    }
    
    public Restaurant? GetRestaurantById(int id)
    {
        return _restaurants.FirstOrDefault(r => r.Id == id);
    }
}