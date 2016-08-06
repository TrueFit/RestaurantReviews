using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ninject;
using RstrntAPI.Business;
using RstrntAPI.Business.Services;
using RstrntAPI.Models.Transforms;
using RstrntAPI.Models.Request;
using RstrntAPI.Models.Response;
using RstrntAPI.Validation;
using FluentValidation;

namespace RstrntAPI.Controllers
{
    [RoutePrefix("api/v1/Restaurant")]
    public class RestaurantController : ApiController
    {
        [HttpGet()]
        [Route("")]
        public RestaurantModelResponse GetAll()
        {
            var response = new RestaurantModelResponse();
            try
            {
                response.Restaurant = ServiceRegistry.Get<IRestaurantService>().GetAll().Select(x => x.ToRequest()).ToList();
                response.HasError = false;
            }
            catch (Exception)
            {
                response.HasError = true;
                response.ErrorMessages = new List<string>() { "Unexpected Error" };
            }
            return response;
        }

        [HttpGet()]
        [Route("{restaurantId:int}/Detail")]
        public RestrLocalResponse FullDescription(int restaurantId)
        {
            var response = new RestrLocalResponse();
            try
            {
                var restaurants = ServiceRegistry.Get<ILocationService>().FullDescription(restaurantId);
                response.Restaurants = restaurants.Select(x => x.ToResponse()).ToList();
                response.HasError = false;
            }
            catch (Exception)
            {
                response.HasError = true;
                response.ErrorMessages = new List<string>() { "Unexpected Error" };
            }
            return response;
        }

        [HttpGet()]
        [Route("{restaurantId:int}/Location/{locationId:int}")]
        public RestrLocalResponse FullDescriptionBranch(int restaurantId, int locationId)
        {
            var response = new RestrLocalResponse();
            try
            {
                var branch = ServiceRegistry.Get<ILocationService>().FullDescriptionBranch(restaurantId, locationId);
                response.Restaurants = new List<LocationDetailResponse> { branch.ToResponse() };
                response.HasError = false;
            }
            catch (Exception)
            {
                response.HasError = true;
                response.ErrorMessages = new List<string>() { "Unexpected Error" };
            }
            return response;
        }

        [HttpGet()]
        [Route("{restaurantId:int}/City/{cityId:int}")]
        public RestrLocalResponse FullDescriptionCity(int restaurantId, int cityId)
        {
            var response = new RestrLocalResponse();
            try
            {
                var restaurants = ServiceRegistry.Get<ILocationService>().FullDescriptionCity(restaurantId, cityId);
                response.Restaurants = restaurants.Select(x => x.ToResponse()).ToList();
                response.HasError = false;
            }
            catch (Exception)
            {
                response.HasError = true;
                response.ErrorMessages = new List<string>() { "Unexpected Error" };
            }
            return response;
        }

        [HttpGet()]
        [Route("City/{cityId:int}")]
        public RestaurantModelResponse ListByCity(int cityId)
        {
            var response = new RestaurantModelResponse();
            try
            {
                response.Restaurant = ServiceRegistry.Get<IRestaurantService>().ListByCity(cityId).Select(x => x.ToRequest()).ToList();
                response.HasError = false;
            }
            catch (Exception)
            {
                response.HasError = true;
                response.ErrorMessages = new List<string>() { "Unexpected Error" };
            }
            return response;
        }

        [HttpGet()]
        [Route("{restaurantId:int}"), Route("")]
        public RestaurantModelResponse Get(int restaurantId)
        {
            var response = new RestaurantModelResponse();
            try
            {
                response.Restaurant = new List<RestaurantRequest> { ServiceRegistry.Get<IRestaurantService>().Get(restaurantId).ToRequest() };
                response.HasError = false;
            }
            catch (Exception)
            {
                response.HasError = true;
                response.ErrorMessages = new List<string>() { "Unexpected Error" };
            }
            return response;
        }

        [HttpPost()]
        [Route("")]
        public RestaurantModelResponse Create(RestaurantRequest restaurant)
        {
            var response = new RestaurantModelResponse();
            try
            {
                var modelValidate = new RestaurantValidator().Validate(restaurant, ruleSet: "Create");
                if (modelValidate.IsValid)
                {
                    response.HasError = false;
                    response.Restaurant = new List<RestaurantRequest> { ServiceRegistry.Get<IRestaurantService>().Create(restaurant.ToDTO()).ToRequest() };
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

        [HttpDelete()]
        [Route("{restaurantId:int}"), Route("")]
        public RestaurantModelResponse Delete(int restaurantId)
        {
            var response = new RestaurantModelResponse();
            try
            {
                response.Restaurant = null;
                response.HasError = !ServiceRegistry.Get<IRestaurantService>().Delete(restaurantId);
            }
            catch (Exception)
            {
                response.HasError = true;
                response.ErrorMessages = new List<string>() { "Unexpected Error" };
            }
            return response;
        }

        [HttpPut()]
        [Route("")]
        public RestaurantModelResponse Update(RestaurantRequest restaurant)
        {
            var response = new RestaurantModelResponse();
            try
            {
                var modelValidate = new RestaurantValidator().Validate(restaurant, ruleSet: "Update");
                if (modelValidate.IsValid)
                {
                    response.Restaurant = new List<RestaurantRequest> { ServiceRegistry.Get<IRestaurantService>().Update(restaurant.ToDTO()).ToRequest() };
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
