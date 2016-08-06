using System.Collections.Generic;
using RstrntAPI.DTO;

namespace RstrntAPI.Repository.Repositories
{
    public interface IRestaurantRepository
    {
        RestaurantDTO Create(RestaurantDTO restaurant);
        bool Delete(RestaurantDTO restaurant);
        bool Delete(int restaurantId);
        List<RestaurantDTO> Find(string term);
        RestaurantDTO Get(int restaurantId);
        RestaurantDTO Get(string restaurantName);
        List<RestaurantDTO> GetAll();
        List<RestaurantDTO> ListByCity(int cityId);
        List<RestaurantDTO> ListByCity(CityDTO city);
        List<RestaurantDTO> ListByUser(UserDTO user);
        List<RestaurantDTO> ListByUser(int userId);
        RestaurantDTO Update(RestaurantDTO restaurant);
    }
}