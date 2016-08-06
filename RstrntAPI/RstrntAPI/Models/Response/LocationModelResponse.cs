using System.Collections.Generic;
using RstrntAPI.Models.Request;

namespace RstrntAPI.Models.Response
{
    public class LocationModelResponse
    {
        public List<LocationRequest> Location { get; set; }
        public bool HasError { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
