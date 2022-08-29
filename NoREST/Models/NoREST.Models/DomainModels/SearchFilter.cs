using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoREST.Models.DomainModels
{
    public class ReviewSearchFilter
    {
        public int? UserId { get; set; }
        public int? RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public bool? IsActive { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Search Criteria: ");
            if (UserId.HasValue) sb.Append($"UserId = {UserId.Value}; ");
            if (RestaurantId.HasValue) sb.Append($"RestaurantId = {RestaurantId.Value}; ");
            if (!string.IsNullOrWhiteSpace(RestaurantName)) sb.Append($"RestaurantName = {RestaurantName}; ");
            if (IsActive.HasValue) sb.Append($"IsActive = {IsActive.Value}; ");

            return sb.ToString();
        }
    }

    public class RestaurantSearchFilter
    {
        public string City { get; set; }
    }
}
