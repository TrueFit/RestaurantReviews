using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.RestaurantModels
{
    public class UpdateRestaurantModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Name must be less than 50 characters long")]
        public string Name { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "City must be less than 50 characters long")]
        public string City { get; set; }
        [Required]
        [RegularExpression("^[A-Z]{2}$", ErrorMessage = "State must be 2-character capitalized state code")]
        public string State { get; set; }
        [Required]
        [RegularExpression("^[0-9]{5}$", ErrorMessage = "Invalid 5-digit zip code")]
        [MaxLength(5, ErrorMessage = "Invalid 5-digit zip code")]
        public string Zipcode { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "StreetAddress1 must be less than 50 characters long")]
        public string StreetAddress1 { get; set; }
        [MaxLength(50, ErrorMessage = "StreetAddress2 must be less than 50 characters long")]
        public string StreetAddress2 { get; set; }
        [RegularExpression("^[0-9]{10}", ErrorMessage = "Phone number must be 10-digit number (including area code) with no formatting")]
        public string PhoneNum { get; set; }
        public string OwnerUserName { get; set; }
    }
}