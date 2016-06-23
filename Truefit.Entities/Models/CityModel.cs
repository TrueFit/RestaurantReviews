using System;

namespace RestaurantReviews.Data.Models
{
    public class CityModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string StateAbbr { get; set; }
    }
}
