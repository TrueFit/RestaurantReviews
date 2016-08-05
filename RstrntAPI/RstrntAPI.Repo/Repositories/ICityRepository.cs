using System.Collections.Generic;
using RstrntAPI.DTO;

namespace RstrntAPI.Repository.Repositories
{
    public interface ICityRepository
    {
        CityDTO Create(CityDTO city);
        int Delete(CityDTO city);
        List<CityDTO> Find(string term);
        CityDTO Get(int cityId);
        List<CityDTO> GetAll();
        List<CityDTO> ListByRestaurant(RestaurantDTO restaurant);
        List<CityDTO> ListByRestaurant(int restaurantId);
        CityDTO Update(CityDTO city);
    }
}