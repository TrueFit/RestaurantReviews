using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Massive;

namespace RstrntAPI.DataAccess.Models
{
    public class Locations : DynamicModel
    {
        public Locations() : base("LocalDatabase", "locations", "id", "", "id") { }
    }

    public class LocationsEntity
    {
        public int? id { get; set; }
        public int restaurant_id { get; set; }
        public int city_id { get; set; }
        public string street_address { get; set; }
    }
}
