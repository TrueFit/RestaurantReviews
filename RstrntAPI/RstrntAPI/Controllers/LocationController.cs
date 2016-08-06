using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ninject;
using RstrntAPI.Business;
using RstrntAPI.Models.Transforms;
using RstrntAPI.Models.Request;
using RstrntAPI.Models.Response;
using RstrntAPI.Business.Services;
using RstrntAPI.Validation;
using FluentValidation;

namespace RstrntAPI.Controllers
{
    [RoutePrefix("api/v1/Location")]
    public class LocationController : ApiController
    {
        [HttpGet()]
        [Route("")]
        public LocationModelResponse GetAll()
        {
            // this isnt very helpful, probably should just be removed
            var response = new LocationModelResponse();
            try
            {
                response.Location = ServiceRegistry.Get<ILocationService>().GetAll().Select(x => x.ToRequest()).ToList();
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
        public LocationModelResponse ListByCity(int cityId)
        {
            var response = new LocationModelResponse();
            try
            {
                response.Location = ServiceRegistry.Get<ILocationService>().ListByCity(cityId).Select(x => x.ToRequest()).ToList();
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
        [Route("Restaurant/{restaurantId:int}")]
        public LocationModelResponse ListByRestaurant(int restaurantId)
        {
            var response = new LocationModelResponse();
            try
            {
                response.Location = ServiceRegistry.Get<ILocationService>().ListByRestaurant(restaurantId).Select(x => x.ToRequest()).ToList();
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
        [Route("{locationId:int}"), Route("")]
        public LocationModelResponse Get(int locationId)
        {
            var response = new LocationModelResponse();
            try
            {
                response.Location = new List<LocationRequest> { ServiceRegistry.Get<ILocationService>().Get(locationId).ToRequest() };
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
        public LocationModelResponse Create(LocationRequest location)
        {
            var response = new LocationModelResponse();
            try
            {
                var modelValidate = new LocationValidator().Validate(location, ruleSet: "Create");
                if (modelValidate.IsValid)
                {
                    response.Location = new List<LocationRequest> { ServiceRegistry.Get<ILocationService>().Create(location.ToDTO()).ToRequest() };
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

        [HttpDelete()]
        [Route("{locationId:int}"), Route("")]
        public LocationModelResponse Delete(int locationId)
        {
            var response = new LocationModelResponse();
            try
            {
                response.Location = null;
                response.HasError = !ServiceRegistry.Get<ILocationService>().Delete(locationId);
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
        public LocationModelResponse Update(LocationRequest location)
        {
            var response = new LocationModelResponse();
            try
            {
                var modelValidate = new LocationValidator().Validate(location, ruleSet: "Update");
                if (modelValidate.IsValid)
                {
                    response.Location = new List<LocationRequest> { ServiceRegistry.Get<ILocationService>().Update(location.ToDTO()).ToRequest() };
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
