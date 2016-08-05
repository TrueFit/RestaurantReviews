using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RstrntAPI.Business;
using RstrntAPI.Business.Repositories;
using RstrntAPI.DTO;

namespace RstrntAPI.Controllers
{
    [RoutePrefix("api/v1/Search")]
    public class SearchController : ApiController
    {
        [HttpGet()]
        [Route("Restaurants")]
        public List<RestaurantDTO> SearchRestaurants(string term)
        {
            return RepoRegistry.Get<IRestaurantRepository>().Find(term);
        }

        [HttpGet()]
        [Route("Reviews")]
        public List<ReviewDTO> SearchReviews(string term)
        {
            return RepoRegistry.Get<IReviewRepository>().Find(term);
        }

        [HttpGet()]
        [Route("Users")]
        public List<UserDTO> SearchUsers(string term)
        {
            return RepoRegistry.Get<IUserRepository>().Find(term);
        }

        [HttpGet()]
        [Route("City")]
        public List<CityDTO> SearchCity(string term)
        {
            return RepoRegistry.Get<ICityRepository>().Find(term);
        }
    }
}
