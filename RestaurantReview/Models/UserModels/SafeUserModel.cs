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
        [MaxLength(20, ErrorMessage = "Username can be no longer than 20 characters")]
        [MinLength(5, ErrorMessage = "Username must be at least 5 characters")]
        [RegularExpression("^[A-Za-z0-9_]+$", ErrorMessage = "UserName must contain alphanumeric characters or '_'")]
        public string UserName { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z0-9_]+([-+.'][A-Za-z0-9_]+)*@[A-Za-z0-9_]+[.][A-Za-z0-9]{2,3}$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public string EndTime { get; set; }
    }
}