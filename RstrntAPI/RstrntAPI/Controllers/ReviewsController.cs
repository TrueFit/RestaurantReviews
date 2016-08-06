using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RstrntAPI.Business;
using RstrntAPI.Business.Services;
using RstrntAPI.DTO;
using RstrntAPI.Models.Request;
using RstrntAPI.Models.Response;
using RstrntAPI.Models.Transforms;
using RstrntAPI.Validation;
using FluentValidation;

namespace RstrntAPI.Controllers
{
    [RoutePrefix("api/v1/Reviews")]
    public class ReviewsController : ApiController
    {
        [HttpGet()]
        [Route("")]
        public ReviewModelResponse GetAll()
        {
            var response = new ReviewModelResponse();
            try
            {
                response.Review = ServiceRegistry.Get<IReviewService>().GetAll().Select(x => x.ToRequest()).ToList();
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
        [Route("{reviewId:int}"), Route("")]
        public ReviewModelResponse Get(int reviewId)
        {
            var response = new ReviewModelResponse();
            try
            {
                response.Review = new List<ReviewRequest> { ServiceRegistry.Get<IReviewService>().Get(reviewId).ToRequest() };
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
        public ReviewModelResponse Create(ReviewRequest review)
        {
            var response = new ReviewModelResponse();
            try
            {
                var modelValidate = new ReviewValidator().Validate(review, ruleSet: "Create");
                if (modelValidate.IsValid)
                {
                    response.Review = new List<ReviewRequest> { ServiceRegistry.Get<IReviewService>().Create(review.ToDTO()).ToRequest() };
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
        [Route("{reviewId:int}"), Route("")]
        public ReviewModelResponse Delete(int reviewId)
        {
            var response = new ReviewModelResponse();
            try
            {
                response.Review = null;
                response.HasError = !ServiceRegistry.Get<IReviewService>().Delete(reviewId);
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
        public ReviewModelResponse Update(ReviewRequest review)
        {
            var response = new ReviewModelResponse();
            try
            {
                var modelValidate = new ReviewValidator().Validate(review, ruleSet: "Update");
                if (modelValidate.IsValid)
                {
                    response.Review = new List<ReviewRequest> { ServiceRegistry.Get<IReviewService>().Update(review.ToDTO()).ToRequest() };
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
