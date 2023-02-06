using Microsoft.AspNetCore.Mvc;
using RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate;
using RestaurantReviews.Domain.AggregatesModel.ReviewAggregate;
using System.ComponentModel.DataAnnotations;

namespace RestaurantReviews.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IRestaurantReviewRepository _restReviewRepo;
        private readonly IReviewRepository _reviewRepo;

        public ReviewsController(IRestaurantReviewRepository restReviewRepo, IReviewRepository reviewRepo)
        {
            _restReviewRepo = restReviewRepo;
            _reviewRepo = reviewRepo;
        }

        [HttpGet]
        [Route("{id}")]
        public Review Get(int id)
        {
            return _reviewRepo.GetById(id);
        }


        [HttpGet]
        public IEnumerable<Review> GetReviews(int? restaurantId, int? userId)
        {
            return _reviewRepo.Get(restaurantId, userId);
        }

        [HttpPost]
        public ActionResult<int> Post([FromBody] Review review)
        {
            try
            {
                return Ok(_restReviewRepo.AddReview(review));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<int> Put(int id, [FromBody] Review review)
        {
            try
            {
                return Ok(_restReviewRepo.UpdateReview(id, review));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id, [Required] int userId)
        {
            try
            {
                _restReviewRepo.DeleteReview(id, userId);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
