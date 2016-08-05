using System.Collections.Generic;
using RstrntAPI.DTO;

namespace RstrntAPI.Repository.Repositories
{
    public interface ILocationRepository
    {
        LocationDTO Create(LocationDTO location);
        bool Delete(LocationDTO location);
        bool Delete(int locationId);
        List<LocationDTO> Find(string term);
        LocationDTO Get(int locationId);
        List<LocationDTO> GetAll();
        List<LocationDTO> ListByCity(int cityId);
        List<LocationDTO> ListByCity(CityDTO city);
        LocationDTO Update(LocationDTO location);
        List<LocationDTO> ListByRestaurant(int restaurantId);
    }
}