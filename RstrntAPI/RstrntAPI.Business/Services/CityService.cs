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
    public class CityService : ICityService
    {
        public CityDTO Get(int cityId)
        {
            return RepoRegistry.Get<ICityRepository>().Get(cityId);           
        }

        public CityDTO Get(string cityKeyedName)
        {
            return RepoRegistry.Get<ICityRepository>().Get(cityKeyedName);
        }

        public CityDTO Create(CityDTO city)
        {
            return RepoRegistry.Get<ICityRepository>().Create(city);
        }

        public CityDTO Update(CityDTO city)
        {
            return RepoRegistry.Get<ICityRepository>().Update(city);
        }

        public bool Delete(CityDTO city)
        {
            return RepoRegistry.Get<ICityRepository>().Delete(city);
        }

        public bool Delete(int cityId)
        {
            return RepoRegistry.Get<ICityRepository>().Delete(cityId);
        }

        public List<CityDTO> GetAll()
        {
            return RepoRegistry.Get<ICityRepository>().GetAll();
        }

        public List<CityDTO> Find(string term)
        {
            return RepoRegistry.Get<ICityRepository>().Find(term);
        }

        public List<CityDTO> ListByRestaurant(int restaurantId)
        {
            return RepoRegistry.Get<ICityRepository>().ListByRestaurant(restaurantId);
        }

        public List<CityDTO> ListByRestaurant(RestaurantDTO restaurant)
        {
            return RepoRegistry.Get<ICityRepository>().ListByRestaurant(restaurant);
        }
    }
}
