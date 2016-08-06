using RstrntAPI.Models.Request;
using System.Collections.Generic;

namespace RstrntAPI.Models.Response
{
    public class LocationDetailResponse
    {
        public int? RestaurantId { get; set; }
        public int? LocationId { get; set; }
        public int? CityId { get; set; }

        public string RestaurantName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }

        public List<ReviewRequest> Reviews { get; set; }
    }
}
