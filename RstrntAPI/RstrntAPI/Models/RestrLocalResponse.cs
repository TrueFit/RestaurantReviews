using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RstrntAPI.DTO;

namespace RstrntAPI.Models
{
    public class RestrLocalResponse
    {
        public int? RestaurantId { get; set; }
        public int? BranchId { get; set; }

        public string RestaurantName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }

        public List<ReviewDTO> Reviews { get; set;}
    }
}
