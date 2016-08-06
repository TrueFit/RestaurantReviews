using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RstrntAPI.Models.Request;

namespace RstrntAPI.Models.Response
{
    public class RestaurantModelResponse
    {
        public List<RestaurantRequest> Restaurant { get; set; }
        public bool HasError { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
