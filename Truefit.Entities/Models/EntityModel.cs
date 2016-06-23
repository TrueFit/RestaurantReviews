using System;

namespace RestaurantReviews.Data.Models
{
    public class EntityModel
    {
        public Guid Guid { get; set; }
        public Guid CityGuid { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string StateAbbr { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Should be set to false for User Submitted entities
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Should be set to true for User submitted entities
        /// Should be set to false by an admin when processed
        /// </summary>
        public bool NeedsReviewed { get; set; }
    }
}
