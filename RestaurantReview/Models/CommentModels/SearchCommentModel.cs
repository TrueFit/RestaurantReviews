using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.CommentModels
{
    public class SearchCommentModel : SearchModel<Comment>
    {
        // Optional search fields
        public int ReviewId { get; set; }
        public string UserName { get; set; }
    }
}