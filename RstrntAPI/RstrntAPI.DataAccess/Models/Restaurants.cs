using Massive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RstrntAPI.DataAccess.Models
{
    public class Restaurants : DynamicModel
    {
        public Restaurants() : base("LocalDatabase", "restaurants", "id", "", "id") { }
    }

    public class RestaurantsEntity
    {
        public int? id { get; set; }
        public string name { get; set; }
    }
}
