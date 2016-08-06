using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Massive;

namespace RstrntAPI.DataAccess.Models
{
    public class City : DynamicModel
    {
        public City() : base("LocalDatabase", "city", "id", "", "id") { }
    }

    public class CityEntity
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string keyed_name { get; set; }
    }
}
