using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.CommentModels
{
    public class CreateCommentModel
    {
        [Required]
        public int ReviewId { get; set; }

        [Required]
        public string Content { get; set; }

        public string UserName { get; set; }
    }
}