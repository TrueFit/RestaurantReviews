using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Core
{
    public class User: BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<RestaurantReview> UserReviews { get; set; }

        public virtual ICollection<Restaurant> CreatedRestaurants { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(FirstName))
                ValidationErrors.Add("First Name is required");

            if (string.IsNullOrEmpty(LastName))
                ValidationErrors.Add("Last Name is required");

            if (string.IsNullOrEmpty(Email))
                ValidationErrors.Add("Email is required");
        }
    }
}
