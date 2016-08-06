using RstrntAPI.DTO;
using RstrntAPI.DataAccess.Models;
using RstrntAPI.DataAccess.Massive;
using System.Collections.Generic;
using System.Linq;
using RstrntAPI.Repository.Transforms;
using System.Dynamic;

namespace RstrntAPI.Repository.Repositories
{
    public class CityRepository : ICityRepository
    {
        #region CRUD

        public CityDTO Get(int cityId)
        {
            var table = new DataAccess.Models.City();
            return table.All(where: "id=@0", args: cityId).Select(x => ((ExpandoObject)x).ToEntity<CityEntity>().ToDTO()).FirstOrDefault();
        }

        public CityDTO Get(string cityKeyedName)
        {
            // Name is unique in the database
            var table = new DataAccess.Models.City();
            return table.All(where: "keyed_name=@0", args: cityKeyedName).Select(x => ((ExpandoObject)x).ToEntity<CityEntity>().ToDTO()).FirstOrDefault();
        }

        public CityDTO Create(CityDTO city)
        {
            if (Get(city.Name) == null)
            {
                var table = new DataAccess.Models.City();
                var returnValue = table.Insert(city.ToEntity());
                return ((ExpandoObject)returnValue).ToEntity<CityEntity>().ToDTO();
            }
            return null;
        }

        public CityDTO Update(CityDTO city)
        {
            var table = new DataAccess.Models.City();
            var returnValue = table.Update(city.ToEntity(), city.Id);
            return Get(city.Id.Value);
        }

        public bool Delete(CityDTO city)
        {
            var table = new DataAccess.Models.City();
            return Delete(city.Id.Value);
        }

        public bool Delete(int cityId)
        {
            var table = new DataAccess.Models.City();
            var returnValue = table.Delete(cityId);
            return returnValue == 0 ? false : true;
        }

        #endregion

        public List<CityDTO> GetAll()
        {
            var table = new DataAccess.Models.City();
            return table.All().Select(x => ((ExpandoObject)x).ToEntity<CityEntity>().ToDTO()).ToList();
        }

        public List<CityDTO> Find(string term)
        {
            var cities = new DataAccess.Models.City().Query(
                "SELECT City.id, City.name, City.keyed_name, Locations.street_address FROM City INNER JOIN Locations ON Locations.city_id = City.id"
                );
            return cities.Where(x => x.name.ToLower().Contains(term.ToLower()) || x.street_address.ToLower().Contains(term.ToLower())).Select(x => ((ExpandoObject)x).ToEntity<CityEntity>().ToDTO()).ToList();
        }

        public List<CityDTO> ListByRestaurant(int restaurantId)
        {
            var rTable = new DataAccess.Models.City();
            // This is Massive's DSL, not SQL
            var restaurants = rTable.Query(
                "SELECT City.id, City.name, City.keyed_name FROM City INNER JOIN Locations ON Locations.city_id = City.id WHERE Location.restaurant_id = @0",
                restaurantId);
            return restaurants.Select(x => ((ExpandoObject)x).ToEntity<CityEntity>().ToDTO()).ToList();
        }

        public List<CityDTO> ListByRestaurant(RestaurantDTO restaurant)
        {
            return ListByRestaurant(restaurant.Id.Value);
        }
    }
}
