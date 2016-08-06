using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RstrntAPI.Business;
using RstrntAPI.Models.Request;
using RstrntAPI.Models.Response;
using RstrntAPI.Models.Transforms;
using RstrntAPI.Business.Services;
using RstrntAPI.Validation;
using FluentValidation;

namespace RstrntAPI.Controllers
{
    [RoutePrefix("api/v1/City")]
    public class CityController : ApiController
    {
        [HttpGet()]
        [Route("")]
        public CityModelResponse GetAll()
        {
            var response = new CityModelResponse();
            try
            {
                response.City = ServiceRegistry.Get<ICityService>().GetAll().Select(x => x.ToRequest()).ToList();
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
        public CityModelResponse ListByRestaurant(int restaurantId)
        {
            var response = new CityModelResponse();
            try
            {
                response.City = ServiceRegistry.Get<ICityService>().GetAll().Select(x => x.ToRequest()).ToList();
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
        [Route("{cityId:int}"), Route("")]
        public CityModelResponse Get(int cityId)
        {
            var response = new CityModelResponse();
            try
            {
                response.City = new List<CityRequest> { ServiceRegistry.Get<ICityService>().Get(cityId).ToRequest() };
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
        public CityModelResponse Create(CityRequest city)
        {
            var response = new CityModelResponse();
            try
            {
                var modelValidate = new CityValidator().Validate(city, ruleSet: "Create");
                if (modelValidate.IsValid)
                {
                    response.City = new List<CityRequest> { ServiceRegistry.Get<ICityService>().Create(city.ToDTO()).ToRequest() };
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
        [Route("{cityId:int}"), Route("")]
        public CityModelResponse Delete(int cityId)
        {
            var response = new CityModelResponse();
            try
            {
                response.City = null;
                response.HasError = !ServiceRegistry.Get<ICityService>().Delete(cityId);
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
        public CityModelResponse Update(CityRequest city)
        {
            var response = new CityModelResponse();
            try
            {
                var modelValidate = new CityValidator().Validate(city, ruleSet: "Update");
                if (modelValidate.IsValid)
                {
                    response.City = new List<CityRequest> { ServiceRegistry.Get<ICityService>().Update(city.ToDTO()).ToRequest() };
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
