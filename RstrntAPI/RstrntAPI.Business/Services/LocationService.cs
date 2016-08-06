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
    public class LocationService : ILocationService
    {
        public LocationDTO Get(int locationId)
        {
            return RepoRegistry.Get<ILocationRepository>().Get(locationId);
        }

        public LocationDTO Create(LocationDTO location)
        {
            return RepoRegistry.Get<ILocationRepository>().Create(location);
        }

        public LocationDTO Update(LocationDTO location)
        {
            return RepoRegistry.Get<ILocationRepository>().Update(location);
        }

        public bool Delete(LocationDTO location)
        {
            return RepoRegistry.Get<ILocationRepository>().Delete(location);
        }

        public bool Delete(int locationId)
        {
            return RepoRegistry.Get<ILocationRepository>().Delete(locationId);
        }

        public List<LocationDTO> GetAll()
        {
            return RepoRegistry.Get<ILocationRepository>().GetAll();
        }

        public List<LocationDTO> Find(string term)
        {
            return RepoRegistry.Get<ILocationRepository>().Find(term);
        }

        public List<LocationDTO> ListByRestaurant(int restaurantId)
        {
            return RepoRegistry.Get<ILocationRepository>().ListByRestaurant(restaurantId);
        }

        public List<LocationDTO> ListByCity(int cityId)
        {
            return RepoRegistry.Get<ILocationRepository>().ListByCity(cityId);
        }

        public List<LocationDTO> ListByCity(CityDTO city)
        {
            return RepoRegistry.Get<ILocationRepository>().ListByCity(city);
        }

        public List<LocationDetailDTO> FullDescription(int restaurantId)
        {
            var responseList = new List<LocationDetailDTO>();
            var restaurant = RepoRegistry.Get<IRestaurantRepository>().Get(restaurantId);
            var location = RepoRegistry.Get<ILocationRepository>().ListByRestaurant(restaurantId);

            if (restaurant != null && location != null)
                foreach (var l in location)
                {
                    var city = RepoRegistry.Get<ICityRepository>().Get(l.CityId);
                    var reviews = RepoRegistry.Get<IReviewRepository>().ListByLocation(l.Id.Value);

                    responseList.Add(
                        new LocationDetailDTO
                        {
                            RestaurantId = restaurant.Id,
                            LocationId = l.Id,
                            CityId = city.Id,
                            RestaurantName = restaurant.Name,
                            StreetAddress = l.StreetAddress,
                            City = city.Name,
                            Reviews = reviews
                        });
                }

            return responseList;
        }

        public LocationDetailDTO FullDescriptionBranch(int restaurantId, int locationId)
        {
            var restaurant = RepoRegistry.Get<IRestaurantRepository>().Get(restaurantId);
            var location = RepoRegistry.Get<ILocationRepository>().Get(locationId);

            if (restaurant != null && location != null)
            {
                var city = RepoRegistry.Get<ICityRepository>().Get(location.CityId) ?? new CityDTO();
                var reviews = RepoRegistry.Get<IReviewRepository>().ListByLocation(location.Id.Value);

                return
                    new LocationDetailDTO
                    {
                        RestaurantId = restaurant.Id,
                        LocationId = location.Id,
                        CityId = city.Id,
                        RestaurantName = restaurant.Name,
                        StreetAddress = location.StreetAddress,
                        City = city.Name,
                        Reviews = reviews
                    };
            }

            return new LocationDetailDTO();
        }

        public List<LocationDetailDTO> FullDescriptionCity(int restaurantId, int cityId)
        {
            var responseList = new List<LocationDetailDTO>();
            var restaurant = RepoRegistry.Get<IRestaurantRepository>().Get(restaurantId);
            var location = RepoRegistry.Get<ILocationRepository>().ListByRestaurant(restaurantId).Where(x => x.CityId == cityId);

            if (restaurant != null && location != null)
                foreach (var l in location)
                {
                    var city = RepoRegistry.Get<ICityRepository>().Get(l.CityId);
                    var reviews = RepoRegistry.Get<IReviewRepository>().ListByLocation(l.Id.Value);

                    responseList.Add(
                        new LocationDetailDTO
                        {
                            RestaurantId = restaurant.Id,
                            LocationId = l.Id,
                            CityId = city.Id,
                            RestaurantName = restaurant.Name,
                            StreetAddress = l.StreetAddress,
                            City = city.Name,
                            Reviews = reviews
                        });
                }

            return responseList;
        }
    }
}
