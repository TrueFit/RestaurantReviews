using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.TagModels
{
    public class CreateDeleteTagModel
    {
        [Required]
        [MaxLength(25, ErrorMessage = "Tags must be no longer than 25 characters")]
        public string TagName { get; set; }

        [Required]
        public Nullable<int> RestaurantId { get; set; }
    }
}