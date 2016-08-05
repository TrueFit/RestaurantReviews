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
    [RoutePrefix("api/v1/Location")]
    public class LocationController : ApiController
    {
        [HttpGet()]
        [Route("")]
        public List<LocationDTO> GetAll()
        {
            // this isnt very helpful, probably should just be removed
            return RepoRegistry.Get<ILocationRepository>().GetAll();
        }

        [HttpGet()]
        [Route("City/{cityId:int}")]
        public List<LocationDTO> ListByCity(int cityId)
        {
            return RepoRegistry.Get<ILocationRepository>().ListByCity(cityId);
        }

        [HttpGet()]
        [Route("Restaurant/{restaurantId:int}")]
        public List<LocationDTO> ListByRestaurant(int restaurantId)
        {
            return RepoRegistry.Get<ILocationRepository>().ListByRestaurant(restaurantId);
        }

        [HttpGet()]
        [Route("{locationId:int}"), Route("")]
        public LocationDTO Get(int locationId)
        {
            return RepoRegistry.Get<ILocationRepository>().Get(locationId);
        }

        [HttpPost()]
        [Route("")]
        public LocationDTO Create(RestaurantDTO location)
        {
            return RepoRegistry.Get<ILocationRepository>().Create(location);
        }

        [HttpDelete()]
        [Route("{locationId:int}"), Route("")]
        public bool Delete(int locationId)
        {
            return RepoRegistry.Get<ILocationRepository>().Delete(locationId);
        }

        [HttpPut()]
        [Route("")]
        public LocationDTO Update(LocationDTO location)
        {
            return RepoRegistry.Get<ILocationRepository>().Update(location);
        }
    }
}
