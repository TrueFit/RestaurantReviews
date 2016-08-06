using System.Collections.Generic;
using RstrntAPI.DTO;

namespace RstrntAPI.Repository.Repositories
{
    public interface ICityRepository
    {
        CityDTO Create(CityDTO city);
        bool Delete(CityDTO city);
        bool Delete(int cityId);
        List<CityDTO> Find(string term);
        CityDTO Get(int cityId);
        CityDTO Get(string cityName);
        List<CityDTO> GetAll();
        List<CityDTO> ListByRestaurant(RestaurantDTO restaurant);
        List<CityDTO> ListByRestaurant(int restaurantId);
        CityDTO Update(CityDTO city);
    }
}