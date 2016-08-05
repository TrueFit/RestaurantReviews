using RstrntAPI.DTO;
using RstrntAPI.DataAccess.Models;
using RstrntAPI.DataAccess.Massive;
using System.Collections.Generic;
using System.Linq;
using RstrntAPI.Repository.Transforms;
using System.Dynamic;

namespace RstrntAPI.Repository.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        #region CRUD

        public LocationDTO Get(int locationId)
        {
            var table = new DataAccess.Models.Locations();
            return table.All(where: "id=@0", args: locationId).Select(x => ((ExpandoObject)x).ToEntity<LocationsEntity>().ToDTO()).FirstOrDefault();
        }

        public LocationDTO Create(LocationDTO location)
        {
            var table = new DataAccess.Models.Locations();
            var returnValue = table.Insert(location.ToEntity());

            return ((ExpandoObject)returnValue).ToEntity<LocationsEntity>().ToDTO();
        }

        public LocationDTO Update(LocationDTO location)
        {
            var table = new DataAccess.Models.Locations();
            var returnValue = table.Update(location.ToEntity(), location.Id);
            return Get(location.Id.Value);
        }

        public bool Delete(LocationDTO location)
        {
            var table = new DataAccess.Models.Locations();
            return Delete(location.Id.Value);
        }

        public bool Delete(int locationId)
        {
            var table = new DataAccess.Models.Locations();
            var returnValue = table.Delete(locationId);
            return returnValue == 0 ? false : true;
        }

        #endregion

        public List<LocationDTO> GetAll()
        {
            var table = new DataAccess.Models.Locations();
            return table.All().Select(x => ((ExpandoObject)x).ToEntity<LocationsEntity>().ToDTO()).ToList();
        }

        public List<LocationDTO> Find(string term)
        {
            // 100% believe Massive is BSing us when they say this is a DSL and not SQL.
            var locations = new DataAccess.Models.Locations().Query(
                "SELECT Locations.*, Restaurants.name as rest_name, City.name as city_name FROM Locations INNER JOIN Restaurants ON Restaurants.id = Locations.restaurant_id INNER JOIN City ON City.id = Locations.city_id"
                );
            return locations.Where(x => x.rest_name.Contains(term) || x.street_address.Contains(term) || x.city_name.Contains(term)).Select(
                x => ((ExpandoObject)x).ToEntity<LocationsEntity>().ToDTO()
                ).ToList();
        }

        public List<LocationDTO> ListByRestaurant(int restaurantId)
        {
            var rTable = new DataAccess.Models.Locations();
            var locations = rTable.Query(
                "SELECT Locations.* FROM Locations WHERE Locations.restaurant_id = @0",
                restaurantId);
            return locations.Select(x => ((ExpandoObject)x).ToEntity<LocationsEntity>().ToDTO()).ToList();
        }

        public List<LocationDTO> ListByCity(int cityId)
        {
            var rTable = new DataAccess.Models.Locations();
            var locations = rTable.Query(
                "SELECT Locations.* FROM Locations WHERE Locations.city_id = @0",
                cityId);
            return locations.Select(x => ((ExpandoObject)x).ToEntity<LocationsEntity>().ToDTO()).ToList();
        }

        public List<LocationDTO> ListByCity(CityDTO city)
        {
            return ListByCity(city.Id.Value);
        }
    }
}
