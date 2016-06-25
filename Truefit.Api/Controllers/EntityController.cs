using System;
using System.Threading.Tasks;
using System.Web.Http;
using RestaurantReviews.Data;
using RestaurantReviews.Data.Models;
using Truefit.Reviews;
using Truefit.Reviews.Models;

namespace Truefit.Api.Controllers
{
    [RoutePrefix("entities")]
    public class EntityController : ApiController
    {
        private readonly IEntityService _entityService;
        private readonly IReviewService _reviewService;

        public EntityController(IEntityService entityService, IReviewService reviewService)
        {
            this._entityService = entityService;
            this._reviewService = reviewService;
        }

        [HttpPost]
        [Route]
        public async Task<IHttpActionResult> Post(EntityModel entity)
        {
            await this._entityService.AddUserEntity(entity);
            return this.Ok();
        }

        [HttpGet]
        [Route("{entityId:guid}")]
        public async Task<IHttpActionResult> GetById(Guid entityId)
        {
            return this.Ok(await this._entityService.GetEntity(entityId));
        }

        [HttpGet]
        [Route("{entityId:guid}/reviews")]
        public async Task<IHttpActionResult> GetReviews(Guid entityId)
        {
            return this.Ok(await this._reviewService.GetByEntity(entityId));
        }

        [HttpPost]
        [Route("{entityId:guid}/reviews")]
        public async Task<IHttpActionResult> PostReview(Guid entityId, ReviewModel review)
        {
            review.EntityGuid = entityId;
            await this._reviewService.AddUserReview(review);
            return this.Ok();
        }
    }
}
