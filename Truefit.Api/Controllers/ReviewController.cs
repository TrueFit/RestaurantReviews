using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Truefit.Api.Controllers
{
    [RoutePrefix("reviews")]
    public class ReviewController : ApiController
    {
        [HttpDelete]
        [Route("{reviewId}")]
        public async Task<IHttpActionResult> DeleteReview(string reviewId, string authToken)
        {
            return this.Ok(reviewId + " " + authToken);
        }
    }
}
