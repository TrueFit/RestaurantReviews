using System;

namespace RestaurantReviews.Data.Models
{
    public class EntityModel
    {
        public Guid Guid { get; set; }
        public Guid CityGuid { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
