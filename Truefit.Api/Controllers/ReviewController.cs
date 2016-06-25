using System;
using System.Threading.Tasks;
using System.Web.Http;
using Truefit.Reviews;
using Truefit.Users;

namespace Truefit.Api.Controllers
{
    [RoutePrefix("reviews")]
    public class ReviewController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IReviewService _reviewService;

        public ReviewController(IUserService userService, IReviewService reviewService)
        {
            this._userService = userService;
            this._reviewService = reviewService;
        }

        [HttpDelete]
        [Route("{reviewId:guid}")]
        public async Task<IHttpActionResult> DeleteReview(Guid reviewId, string authToken)
        {
            var user = await this._userService.Authenticate(authToken);
            var success = await this._reviewService.RemoveUserReview(reviewId, user.Guid);
            return success ? (IHttpActionResult) this.Ok() : this.NotFound();
        }
    }
}
