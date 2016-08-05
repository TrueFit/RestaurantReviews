using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RstrntAPI.DTO
{
    public class ReviewDTO
    {
        public int? Id { get; set; }
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
        public string Subject { get; set; } // cannot be null
        public string Body { get; set; }
    }
}
