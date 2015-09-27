using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models.TagModels
{
    public class DisplayTagModel
    {
        public string TagName { get; set; }
        public Nullable<int> RestaurantId { get; set; }
    }
}