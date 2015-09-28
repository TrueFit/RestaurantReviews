using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.UserModels
{
    public class LogoutUserModel
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Username can be no longer than 20 characters")]
        [MinLength(5, ErrorMessage = "Username must be at least 5 characters")]
        [RegularExpression("^[A-Za-z0-9_]+$", ErrorMessage = "UserName must contain alphanumeric characters or '_'")]
        public string UserName { get; set; }
    }
}