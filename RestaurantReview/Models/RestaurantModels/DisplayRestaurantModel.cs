using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.CustomRestRevModels
{
    public class DisplayRestaurantModel
    {
        public DisplayRestaurantModel()
        {
            this.ReviewIds = new List<int>();
            this.Tags = new List<string>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string PhoneNum { get; set; }
        public string OwnerUserName { get; set; }
    
        public virtual List<int> ReviewIds { get; set; }
        public virtual List<string> Tags { get; set; }
    }
}