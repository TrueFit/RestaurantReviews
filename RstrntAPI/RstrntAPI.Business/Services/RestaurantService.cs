using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RstrntAPI.Repository;
using RstrntAPI.Repository.Repositories;
using RstrntAPI.DTO;

namespace RstrntAPI.Business.Services
{
    public class RestaurantService : IRestaurantService
    {
        public RestaurantDTO Get(int restaurantId)
        {
            return RepoRegistry.Get<IRestaurantRepository>().Get(restaurantId);
        }

        public RestaurantDTO Get(string restaurantKeyedName)
        {
            return RepoRegistry.Get<IRestaurantRepository>().Get(restaurantKeyedName);
        }

        public RestaurantDTO Create(RestaurantDTO restaurant)
        {
            return RepoRegistry.Get<IRestaurantRepository>().Create(restaurant);
        }

        public RestaurantDTO Update(RestaurantDTO restaurant)
        {
            return RepoRegistry.Get<IRestaurantRepository>().Update(restaurant);
        }

        public bool Delete(RestaurantDTO restaurant)
        {
            return RepoRegistry.Get<IRestaurantRepository>().Delete(restaurant);
        }

        public bool Delete(int restaurantId)
        {
            return RepoRegistry.Get<IRestaurantRepository>().Delete(restaurantId);
        }

        public List<RestaurantDTO> GetAll()
        {
            return RepoRegistry.Get<IRestaurantRepository>().GetAll();
        }

        public List<RestaurantDTO> Find(string term)
        {
            return RepoRegistry.Get<IRestaurantRepository>().Find(term);
        }

        public List<RestaurantDTO> ListByCity(int cityId)
        {
            return RepoRegistry.Get<IRestaurantRepository>().ListByCity(cityId);
        }

        public List<RestaurantDTO> ListByCity(CityDTO city)
        {
            return RepoRegistry.Get<IRestaurantRepository>().ListByCity(city);
        }

        public List<RestaurantDTO> ListByUser(int userId)
        {
            return RepoRegistry.Get<IRestaurantRepository>().ListByUser(userId);
        }

        public List<RestaurantDTO> ListByUser(UserDTO user)
        {
            return RepoRegistry.Get<IRestaurantRepository>().ListByUser(user);
        }
    }
}
