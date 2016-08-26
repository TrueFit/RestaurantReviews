using System.Collections.Generic;

namespace RestaurantReviews.Service
{
    public class ServiceCallResult
    {
        public List<string> ValidationErrors { get; internal set; }
        public object ResultObject { get; set; }

        public ServiceCallResult()
        {
            ValidationErrors = new List<string>();
        }
    }
}
