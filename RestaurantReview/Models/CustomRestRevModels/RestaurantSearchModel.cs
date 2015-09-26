using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.CustomRestRevModels
{
    public class RestaurantSearchModel
    {
        // Optional search fields
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string PhoneNum { get; set; }
        public string MinRating { get; set; }
        public string Tag { get; set; }

        // Search options
        public int NumRestaurants { get; set; }
        public int PageNum { get; set; }
        public string OrderBy { get; set; }
        public string Order { get; set; }
    }
}