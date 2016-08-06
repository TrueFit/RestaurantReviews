using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RstrntAPI.Business;
using RstrntAPI.Business.Services;
using RstrntAPI.Models.Request;
using RstrntAPI.Models.Response;
using RstrntAPI.Models.Transforms;
using RstrntAPI.Validation;

namespace RstrntAPI.Controllers
{
    [RoutePrefix("api/v1/Search")]
    public class SearchController : ApiController
    {
        [HttpGet()]
        [Route("Restaurants")]
        public RestaurantModelResponse SearchRestaurants(string term)
        {
            var response = new RestaurantModelResponse();
            try
            {
                var modelValidate = new SearchValidator().Validate(term);
                if (modelValidate.IsValid)
                {
                    response.Restaurant = ServiceRegistry.Get<IRestaurantService>().Find(term).GroupBy(x => x.Id).Select(x => x.First().ToRequest()).ToList();
                    response.HasError = false;
                }
                else
                {
                    response.HasError = true;
                    response.ErrorMessages = modelValidate.Errors.Select(x => x.ErrorMessage).ToList();
                }
            }
            catch (Exception)
            {
                response.HasError = true;
                response.ErrorMessages = new List<string>() { "Unexpected Error" };
            }
            return response;
        }

        [HttpGet()]
        [Route("Reviews")]
        public ReviewModelResponse SearchReviews(string term)
        {
            var response = new ReviewModelResponse();
            try
            {
                var modelValidate = new SearchValidator().Validate(term);
                if (modelValidate.IsValid)
                {
                    response.Review = ServiceRegistry.Get<IReviewService>().Find(term).Select(x => x.ToRequest()).ToList();
                    response.HasError = false;
                }
                else
                {
                    response.HasError = true;
                    response.ErrorMessages = modelValidate.Errors.Select(x => x.ErrorMessage).ToList();
                }
            }
            catch (Exception)
            {
                response.HasError = true;
                response.ErrorMessages = new List<string>() { "Unexpected Error" };
            }
            return response;
        }

        [HttpGet()]
        [Route("Users")]
        public UserModelResponse SearchUsers(string term)
        {
            var response = new UserModelResponse();
            try
            {
                var modelValidate = new SearchValidator().Validate(term);
                if (modelValidate.IsValid)
                {
                    response.User = ServiceRegistry.Get<IUserService>().Find(term).Select(x => x.ToRequest()).ToList();
                    response.HasError = false;
                }
                else
                {
                    response.HasError = true;
                    response.ErrorMessages = modelValidate.Errors.Select(x => x.ErrorMessage).ToList();
                }
            }
            catch (Exception)
            {
                response.HasError = true;
                response.ErrorMessages = new List<string>() { "Unexpected Error" };
            }
            return response;
        }

        [HttpGet()]
        [Route("City")]
        public CityModelResponse SearchCity(string term)
        {
            var response = new CityModelResponse();
            try
            {
                var modelValidate = new SearchValidator().Validate(term);
                if (modelValidate.IsValid)
                {
                    response.City = ServiceRegistry.Get<ICityService>().Find(term).GroupBy(x => x.Id).Select(x => x.First().ToRequest()).ToList();
                    response.HasError = false;
                }
                else
                {
                    response.HasError = true;
                    response.ErrorMessages = modelValidate.Errors.Select(x => x.ErrorMessage).ToList();
                }
            }
            catch (Exception)
            {
                response.HasError = true;
                response.ErrorMessages = new List<string>() { "Unexpected Error" };
            }
            return response;
        }
    }
}
