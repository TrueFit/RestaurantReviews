using System.Collections.Generic;
using RstrntAPI.DTO;

namespace RstrntAPI.Business.Repositories
{
    public interface ILocationRepository
    {
        LocationDTO Create(LocationDTO location);
        int Delete(LocationDTO location);
        List<LocationDTO> Find(string term);
        LocationDTO Get(int locationId);
        List<LocationDTO> GetAll();
        List<LocationDTO> ListByCity(int cityId);
        List<LocationDTO> ListByCity(CityDTO city);
        LocationDTO Update(LocationDTO location);
        List<LocationDTO> ListByRestaurant(int restaurantId);
    }
}