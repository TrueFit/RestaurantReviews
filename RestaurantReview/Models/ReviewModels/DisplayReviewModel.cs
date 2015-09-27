using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.CustomRestRevModels
{
    public class DisplayReviewModel
    {
        public DisplayReviewModel()
        {
            this.CommentIds = new List<int>();
        }
    
        public int Id { get; set; }
        
        public string UserName { get; set; }
        
        [Required]
        public int RestaurantId { get; set; }
        
        [Required]
        [Range(0.0, 10.0, ErrorMessage = "Ratings must be between 0.0 and 10.0")]
        public decimal Rating { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        public string Timestamp { get; set; }
        
        public List<int> CommentIds { get; set; }
        
    }
}