using System.Collections.Generic;
using System.Data.Entity.Spatial;

namespace RestaurantReviews.Core
{
    public class Restaurant: TrackableModel
    {
        public string RestaurantName { get; set; }
        
        public string Description { get; set; }

        //for this I would typically create another object and have it either be a single reference or multiple references like tags. I
        //wanted to keep it simple and not get too carried away with this project.
        public string FoodType { get; set; }

        public bool ServesAlcohol { get; set; }

        public bool KidFriendly { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        //Typically a reference with states as another object. Also, would probably add country but for now just doing a quick string value
        public string State { get; set; }

        public string PostalCode { get; set; }

        public DbGeography GeoLocation { get; set; }

        public virtual ICollection<RestaurantReview> Reviews { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(RestaurantName))
                ValidationErrors.Add("Restaurant name is required");
            
            if (RestaurantName.Length > 200)
                ValidationErrors.Add("Restaurant name cannot exceed 200 characters");

            if (string.IsNullOrEmpty(City) || string.IsNullOrEmpty(State) || string.IsNullOrEmpty(PostalCode))
                ValidationErrors.Add("City, State, and Postal Code fields are required");

            if (CreatorId == 0)
                ValidationErrors.Add("Creator is required");
        }
    }
}
