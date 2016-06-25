using System;
using System.Threading.Tasks;
using System.Web.Http;
using Truefit.Reviews;
using Truefit.Users;

namespace Truefit.Api.Controllers
{
    [RoutePrefix("users")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IReviewService _reviewService;

        public UserController(IUserService userService, IReviewService reviewService)
        {
            this._userService = userService;
            this._reviewService = reviewService;
        }

        [HttpGet]
        [Route("authenticate")]
        public async Task<IHttpActionResult> Authenticate(string authToken)
        {
            return this.Ok(await this._userService.Authenticate(authToken));
        }

        [HttpGet]
        [Route("{userId:guid}/reviews")]
        public async Task<IHttpActionResult> GetReviews(Guid userId)
        {
            return this.Ok();
        }
    }
}
