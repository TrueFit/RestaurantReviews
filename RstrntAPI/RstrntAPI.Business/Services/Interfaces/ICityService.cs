using System.Collections.Generic;
using RstrntAPI.DTO;

namespace RstrntAPI.Business.Services
{
    public interface ICityService
    {
        CityDTO Create(CityDTO city);
        bool Delete(int cityId);
        bool Delete(CityDTO city);
        List<CityDTO> Find(string term);
        CityDTO Get(string cityKeyedName);
        CityDTO Get(int cityId);
        List<CityDTO> GetAll();
        List<CityDTO> ListByRestaurant(RestaurantDTO restaurant);
        List<CityDTO> ListByRestaurant(int restaurantId);
        CityDTO Update(CityDTO city);
    }
}