using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantReviews.API.Models {
    public class RestaurantDTO {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public string WebsiteURL { get; set; }
    }
}