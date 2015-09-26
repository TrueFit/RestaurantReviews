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
        [MaxLength(50)]
        public string UserName { get; set; }
    }
}