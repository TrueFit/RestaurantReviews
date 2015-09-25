using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.UserModels
{
    public class SafeUserModel
    {
        [Required]
        [MaxLength(50)]
        [RegularExpression("^\\w{5, 50}$", ErrorMessage = "UserName must contain alphanumeric characters or \"_\" and be between 5 and 50 characters")]
        public string UserName { get; set; }
        
        [Required]
        public string Email { get; set; }
    }
}