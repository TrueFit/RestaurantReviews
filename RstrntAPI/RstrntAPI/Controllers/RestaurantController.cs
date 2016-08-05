using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ninject;
using RstrntAPI.Business;
using RstrntAPI.Business.Repositories;
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
        [Route("{restaurantId:int}/FullDetail")]
        public List<RestrLocalResponse> FullDescription(int restaurantId)
        {
            // Would be better in the business layer
            var responseList = new List<RestrLocalResponse>();
            var restaurant = RepoRegistry.Get<IRestaurantRepository>().Get(restaurantId);
            var location = RepoRegistry.Get<ILocationRepository>().ListByRestaurant(restaurantId);

            foreach(var l in location)
            {
                var city = RepoRegistry.Get<ICityRepository>().Get(l.CityId);
                var reviews = RepoRegistry.Get<IReviewRepository>().ListByLocation(l.Id.Value);

                responseList.Add(
                    new RestrLocalResponse
                    {
                        RestaurantId = restaurant.Id,
                        BranchId = l.Id,
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
            return RepoRegistry.Get<IRestaurantRepository>().Update(restaurant);
        }
    }
}
