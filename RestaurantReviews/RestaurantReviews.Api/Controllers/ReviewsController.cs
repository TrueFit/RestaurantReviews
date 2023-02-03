using Microsoft.AspNetCore.Mvc;
using RestaurantReviews.Domain.AggregatesModel.ReviewAggregate;

namespace RestaurantReviews.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewsController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public Review Get(int id)
        {
            return _reviewRepository.GetById(id);
        }


        [HttpGet]
        public IEnumerable<Review> GetReviews(int? restaurantId, int? userId)
        {
            return _reviewRepository.Get(restaurantId, userId);
        }

        [HttpPost]
        public int Post([FromBody] Review review)
        {
            return _reviewRepository.Insert(review);
        }

        [HttpPut]
        [Route("{id}")]
        public int Put(int id, [FromBody] Review review)
        {
            return _reviewRepository.Update(id, review);
        }

        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id)
        {
            _reviewRepository.Delete(id);
        }
    }
}
