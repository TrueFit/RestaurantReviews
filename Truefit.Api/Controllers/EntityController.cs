using System;
using System.Threading.Tasks;
using System.Web.Http;
using RestaurantReviews.Data.Models;
using Truefit.Reviews.Models;

namespace Truefit.Api.Controllers
{
    [RoutePrefix("entities")]
    public class EntityController : ApiController
    {
        [HttpPost]
        [Route]
        public async Task<IHttpActionResult> Post(EntityModel entity)
        {
            return this.Ok(entity);
        }

        [HttpGet]
        [Route("{entityId:guid}")]
        public async Task<IHttpActionResult> GetById(Guid? entityId)
        {
            return this.Ok(entityId);
        }

        [HttpGet]
        [Route("{entityId:guid}/reviews")]
        public async Task<IHttpActionResult> GetReviews(Guid? entityId)
        {
            return this.Ok(entityId + "reviews");
        }

        [HttpPost]
        [Route("{entityId:guid}/reviews")]
        public async Task<IHttpActionResult> PostReview(Guid? entityId, ReviewModel review)
        {
            return this.Ok(review);
        }
    }
}
