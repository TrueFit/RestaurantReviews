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
            await this.DeleteReview(reviewId, authToken);
            return this.Ok();
        }
    }
}
