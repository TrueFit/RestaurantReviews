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
using FluentValidation;

namespace RstrntAPI.Controllers
{
    [RoutePrefix("api/v1/User")]
    public class UserController : ApiController
    {
        [HttpGet()]
        [Route("")]
        public UserModelResponse GetAll()
        {
            var response = new UserModelResponse();
            try
            {
                response.User = ServiceRegistry.Get<IUserService>().GetAll().Select(x => x.ToRequest()).ToList();
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
        [Route("{userId:int}"), Route("")]
        public UserModelResponse Get(int userId)
        {
            var response = new UserModelResponse();
            try
            {
                response.User = new List<UserRequest> { ServiceRegistry.Get<IUserService>().Get(userId).ToRequest() };
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
        [Route("{userId:int}/Reviews")]
        public UserReviewsResponse GetReviews(int userId)
        {
            var response = new UserReviewsResponse();
            try
            {
                var user = ServiceRegistry.Get<IUserService>().Get(userId);
                var reviews = ServiceRegistry.Get<IReviewService>().ListByUser(userId);
                response.User = new List<UserRequest> { user.ToRequest() };
                response.Reviews = reviews.Select(x => x.ToRequest()).ToList();
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
        public UserModelResponse Create(UserRequest user)
        {
            var response = new UserModelResponse();
            try
            {

                var modelValidate = new UserValidator().Validate(user, ruleSet: "Create");
                if (modelValidate.IsValid)
                {
                    response.User = new List<UserRequest> { ServiceRegistry.Get<IUserService>().Create(user.ToDTO()).ToRequest() };
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
        [Route("{userId:int}"), Route("")]
        public UserModelResponse Delete(int userId)
        {
            var response = new UserModelResponse();
            try
            {
                response.User = null;
                response.HasError = !ServiceRegistry.Get<IUserService>().Delete(userId);
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
        public UserModelResponse Update(UserRequest user)
        {
            var response = new UserModelResponse();
            try
            {
                var modelValidate = new UserValidator().Validate(user, ruleSet: "Update");
                if (modelValidate.IsValid)
                {
                    response.User = new List<UserRequest> { ServiceRegistry.Get<IUserService>().Update(user.ToDTO()).ToRequest() };
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
