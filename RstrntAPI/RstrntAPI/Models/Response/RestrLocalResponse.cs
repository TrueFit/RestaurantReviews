using System.Collections.Generic;

namespace RstrntAPI.Models.Response
{
    public class RestrLocalResponse
    {
        public List<LocationDetailResponse> Restaurants { get; set; }
        public bool HasError { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
