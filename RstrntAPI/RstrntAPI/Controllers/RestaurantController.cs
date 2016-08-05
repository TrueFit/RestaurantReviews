using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ninject;
using RstrntAPI.Repository;
using RstrntAPI.Repository.Repositories;
using RstrntAPI.DTO;
using RstrntAPI.Models;

namespace RstrntAPI.Controllers
{
    [RoutePrefix("api/v1/Restaurant")]
    public class RestaurantController : ApiController
    {
        [HttpGet()]
        [Route("")]
        public List<RestaurantDTO> GetAll()
        {
            return RepoRegistry.Get<IRestaurantRepository>().GetAll();
        }

        [HttpGet()]
        [Route("{restaurantId:int}/Detail")]
        public List<RestrLocalResponse> FullDescription(int restaurantId)
        {
            var responseList = new List<RestrLocalResponse>();
            var restaurant = RepoRegistry.Get<IRestaurantRepository>().Get(restaurantId);
            var location = RepoRegistry.Get<ILocationRepository>().ListByRestaurant(restaurantId);

            if (restaurant != null && location != null)
                foreach (var l in location)
                {
                    var city = RepoRegistry.Get<ICityRepository>().Get(l.CityId);
                    var reviews = RepoRegistry.Get<IReviewRepository>().ListByLocation(l.Id.Value);

                    responseList.Add(
                        new RestrLocalResponse
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

        [HttpGet()]
        [Route("{restaurantId:int}/Location/{locationId:int}")]
        public RestrLocalResponse FullDescriptionBranch(int restaurantId, int locationId)
        {
            var restaurant = RepoRegistry.Get<IRestaurantRepository>().Get(restaurantId);
            var location = RepoRegistry.Get<ILocationRepository>().Get(locationId);

            if (restaurant != null && location != null)
            {
                var city = RepoRegistry.Get<ICityRepository>().Get(location.CityId) ?? new CityDTO();
                var reviews = RepoRegistry.Get<IReviewRepository>().ListByLocation(location.Id.Value);

                return
                    new RestrLocalResponse
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

            return new RestrLocalResponse();
        }

        [HttpGet()]
        [Route("{restaurantId:int}/City/{cityId:int}")]
        public List<RestrLocalResponse> FullDescriptionCity(int restaurantId, int cityId)
        {
            var responseList = new List<RestrLocalResponse>();
            var restaurant = RepoRegistry.Get<IRestaurantRepository>().Get(restaurantId) ?? new RestaurantDTO();
            var location = RepoRegistry.Get<ILocationRepository>().ListByRestaurant(restaurantId).Where(x => x.CityId == cityId);

            if (restaurant != null && location != null)
                foreach (var l in location)
                {
                    var city = RepoRegistry.Get<ICityRepository>().Get(l.CityId);
                    var reviews = RepoRegistry.Get<IReviewRepository>().ListByLocation(l.Id.Value);

                    responseList.Add(
                        new RestrLocalResponse
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

        [HttpGet()]
        [Route("City/{cityId:int}")]
        public List<RestaurantDTO> ListByCity(int cityId)
        {
            return RepoRegistry.Get<IRestaurantRepository>().ListByCity(cityId);
        }

        [HttpGet()]
        [Route("{restaurantId:int}"), Route("")]
        public RestaurantDTO Get(int restaurantId)
        {
            return RepoRegistry.Get<IRestaurantRepository>().Get(restaurantId);
        }

        [HttpPost()]
        [Route("")]
        public RestaurantDTO Create(RestaurantDTO restaurant)
        {
            // Validation
            return RepoRegistry.Get<IRestaurantRepository>().Create(restaurant);
        }

        [HttpDelete()]
        [Route("{restaurantId:int}"), Route("")]
        public bool Delete(int restaurantId)
        {
            return RepoRegistry.Get<IRestaurantRepository>().Delete(restaurantId);
        }

        [HttpPut()]
        [Route("")]
        public RestaurantDTO Update(RestaurantDTO restaurant)
        {
            // Validation
            return RepoRegistry.Get<IRestaurantRepository>().Update(restaurant);
        }
    }
}
