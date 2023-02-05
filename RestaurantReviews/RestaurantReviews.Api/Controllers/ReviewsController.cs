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
        public int Post([FromBody] Review review)
        {
            return _restReviewRepo.AddReview(review);
        }

        [HttpPut]
        [Route("{id}")]
        public int Put(int id, [FromBody] Review review)
        {
            return _restReviewRepo.UpdateReview(id, review);
        }

        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id, [Required] int userId)
        {
            _restReviewRepo.DeleteReview(id, userId);
        }
    }
}
