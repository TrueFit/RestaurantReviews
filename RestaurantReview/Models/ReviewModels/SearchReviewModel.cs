using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.ReviewModels
{
    public class SearchReviewModel : SearchModel<Review>
    {
        // Optional search fields
        public string UserName { get; set; }
        public int RestaurantId { get; set; }
    }
}