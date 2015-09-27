using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.ReviewModels
{
    public class CreateReviewModel
    {
        [Required]
        public int RestaurantId { get; set; }

        [Required]
        [Range(0.0, 10.0, ErrorMessage = "Ratings must be between 0.0 and 10.0")]
        public decimal Rating { get; set; }

        [Required]
        public string Content { get; set; }

        public string UserName { get; set; }
    }
}