using RstrntAPI.Models.Request;
using System.Collections.Generic;

namespace RstrntAPI.Models.Response
{
    public class UserReviewsResponse
    {
        public List<UserRequest> User { get; set; }
        public List<ReviewRequest> Reviews { get; set; }
        public bool HasError { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
