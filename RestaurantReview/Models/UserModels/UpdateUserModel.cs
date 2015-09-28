using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.UserModels
{
    public class UpdateUserModel
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Password can be no longer than 30 characters")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters")]
        public string NewPassword { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Password can be no longer than 30 characters")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters")]
        public string OldPassword { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z0-9_]+([-+.'][A-Za-z0-9_]+)*@[A-Za-z0-9_]+[.][A-Za-z0-9]{2,3}$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
    }
}