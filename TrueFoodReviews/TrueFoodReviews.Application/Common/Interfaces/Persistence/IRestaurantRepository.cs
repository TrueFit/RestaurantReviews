using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Application.Common.Interfaces.Persistence;

public interface IRestaurantRepository
{
    void Add(Restaurant restaurant);
    List<Restaurant> GetAll();
    List<Restaurant> GetRestaurantByName(string requestName);
    List<Restaurant> GetRestaurantByCity(string city);
    Restaurant? GetRestaurantById(int id);
}