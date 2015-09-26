using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.ResponseModels
{
    public class UserLoginResponseModel
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
    }
}