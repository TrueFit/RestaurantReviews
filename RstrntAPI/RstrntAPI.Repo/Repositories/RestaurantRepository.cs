using RstrntAPI.DTO;
using RstrntAPI.DataAccess.Models;
using RstrntAPI.DataAccess.Massive;
using System.Collections.Generic;
using System.Linq;
using RstrntAPI.Repository.Transforms;
using System.Dynamic;

namespace RstrntAPI.Repository.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        #region CRUD

        public RestaurantDTO Get(int restaurantId)
        {
            var table = new DataAccess.Models.Restaurants();
            return table.All(where: "id=@0", args:restaurantId).Select(x => ((ExpandoObject)x).ToEntity<RestaurantsEntity>().ToDTO()).FirstOrDefault();
        }

        public RestaurantDTO Get(string restaurantKeyedName)
        {
            var table = new DataAccess.Models.Restaurants();
            return table.All(where: "keyed_name=@0", args: restaurantKeyedName).Select(x => ((ExpandoObject)x).ToEntity<RestaurantsEntity>().ToDTO()).FirstOrDefault();
        }

        public RestaurantDTO Create(RestaurantDTO restaurant)
        {
            if (Get(restaurant.KeyedName) == null)
            {
                var table = new DataAccess.Models.Restaurants();
                var returnValue = table.Insert(restaurant.ToEntity());

                return ((ExpandoObject)returnValue).ToEntity<RestaurantsEntity>().ToDTO();
            }
            return null;
        }

        public RestaurantDTO Update(RestaurantDTO restaurant)
        {
            var table = new DataAccess.Models.Restaurants();
            var returnValue = table.Update(restaurant.ToEntity(), restaurant.Id);
            return Get(restaurant.Id.Value);
        }

        public bool Delete(RestaurantDTO restaurant)
        {
            var table = new DataAccess.Models.Restaurants();
            var returnValue = Delete(restaurant.Id.Value);
            return returnValue;
        }

        public bool Delete(int restaurantId)
        {
            var table = new DataAccess.Models.Restaurants();
            var value = Get(restaurantId);
            if (value != null)
                return table.Delete(restaurantId) != 0 ? true : false;
            return false;
        }

        #endregion

        public List<RestaurantDTO> GetAll()
        {
            var table = new DataAccess.Models.Restaurants();
            return table.All().Select(x => ((ExpandoObject)x).ToEntity<RestaurantsEntity>().ToDTO()).ToList();
        }

        public List<RestaurantDTO> Find(string term)
        {
            var restaurants = new DataAccess.Models.Restaurants().Query(
                "SELECT Restaurants.id, Restaurants.name, Restaurants.keyed_name, Locations.street_address FROM Restaurants INNER JOIN Locations ON Locations.restaurant_id = Restaurants.id"
                );
            return restaurants.Where(x => x.name.Contains(term) || x.street_address.Contains(term)).Select(x => ((ExpandoObject)x).ToEntity<RestaurantsEntity>().ToDTO()).ToList();
        }

        public List<RestaurantDTO> ListByCity(int cityId)
        {
            var rTable = new DataAccess.Models.Restaurants();            
            // Massive claims this isn't SQL, but rather a DSL, but sure looks like SQL to me.
            var restaurants = rTable.Query(
                "SELECT Restaurants.id, Restaurants.name, Restaurants.keyed_name FROM Restaurants INNER JOIN Locations ON Locations.restaurant_id = Restaurants.id WHERE Locations.city_id = @0",
                cityId);
            return restaurants.Select(x => ((ExpandoObject)x).ToEntity<RestaurantsEntity>().ToDTO()).ToList();
        }

        public List<RestaurantDTO> ListByCity(CityDTO city)
        {
            return ListByCity(city.Id.Value);
        }

        public List<RestaurantDTO> ListByUser(int userId)
        {
            var rTable = new DataAccess.Models.Restaurants();
            var restaurants = rTable.Query(
                "SELECT Restaurants.id, Restaurants.name, Restaurants.keyed_name FROM Restaurants INNER JOIN Reviews ON Reviews.restaurant_id = Restaurants.id WHERE Reviews.id = @0",
                userId);
            return restaurants.Select(x => ((ExpandoObject)x).ToEntity<RestaurantsEntity>().ToDTO()).ToList();
        }

        public List<RestaurantDTO> ListByUser(UserDTO user)
        {
            return ListByUser(user.Id.Value);
        }
    }
}
