using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.CommentModels
{
    public class DisplayCommentModel
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public string Timestamp { get; set; }
    }
}