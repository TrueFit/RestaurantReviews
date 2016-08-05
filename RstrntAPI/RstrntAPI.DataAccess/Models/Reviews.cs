using Massive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RstrntAPI.DataAccess.Models
{
    public class Reviews : DynamicModel
    {
        public Reviews() : base("LocalDatabase", "reviews", "id", "", "id") { }
    }

    public class ReviewsEntity
    {
        public int? id { get; set; }
        public int restaurant_id { get; set; }
        public int user_id { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }
}
