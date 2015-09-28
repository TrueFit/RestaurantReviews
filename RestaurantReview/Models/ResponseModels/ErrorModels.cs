using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.ResponseModels
{
    public class ErrorModel
    {
        public string error { get; set; }
    }

    public static class ErrorModels
    {
        // The request is missing a required parameter
        public static ErrorModel InvalidRequest = new ErrorModel { error = "invalid_request" };

        //The provided authorization grant or refresh token is invalid
        public static ErrorModel InvalidGrant = new ErrorModel { error = "invalid_grant" };

        // The resource owner or authorization server denied the request
        public static ErrorModel AccessDenied = new ErrorModel { error = "access_denied" };
    }
}