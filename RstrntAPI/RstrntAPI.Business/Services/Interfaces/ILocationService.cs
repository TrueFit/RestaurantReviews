using System.Collections.Generic;
using RstrntAPI.DTO;

namespace RstrntAPI.Business.Services
{
    public interface ILocationService
    {
        LocationDTO Create(LocationDTO location);
        bool Delete(LocationDTO location);
        bool Delete(int locationId);
        List<LocationDTO> Find(string term);
        List<LocationDetailDTO> FullDescription(int restaurantId);
        LocationDetailDTO FullDescriptionBranch(int restaurantId, int locationId);
        List<LocationDetailDTO> FullDescriptionCity(int restaurantId, int cityId);
        LocationDTO Get(int locationId);
        List<LocationDTO> GetAll();
        List<LocationDTO> ListByCity(int cityId);
        List<LocationDTO> ListByCity(CityDTO city);
        List<LocationDTO> ListByRestaurant(int restaurantId);
        LocationDTO Update(LocationDTO location);
    }
}