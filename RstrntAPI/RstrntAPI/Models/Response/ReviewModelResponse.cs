using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RstrntAPI.Models.Request;

namespace RstrntAPI.Models.Response
{
    public class ReviewModelResponse
    {
        public List<ReviewRequest> Review { get; set; }
        public bool HasError { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
