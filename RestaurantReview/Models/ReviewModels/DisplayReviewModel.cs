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
        public int RestaurantId { get; set; }
        public decimal Rating { get; set; }
        public string Content { get; set; }
        public string Timestamp { get; set; }
        public List<int> CommentIds { get; set; }
        
    }
}