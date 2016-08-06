using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RstrntAPI.DTO
{
    public class LocationDTO
    {
        public int? Id { get; set; }
        public int CityId { get; set; }
        public int RestaurantId { get; set; }
        public string StreetAddress { get; set; }
    }
}
