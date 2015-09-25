using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.UserModels
{
    public class CreateUserModel
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
        
        [Required]
        [MaxLength(50)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Email { get; set; }
    }
}