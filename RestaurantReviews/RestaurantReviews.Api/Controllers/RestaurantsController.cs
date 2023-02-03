using Microsoft.AspNetCore.Mvc;
using RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate;
using RestaurantReviews.Domain.AggregatesModel.ReviewAggregate;
using System.ComponentModel.DataAnnotations;

namespace RestaurantReviews.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IReviewRepository _reviewRepository;

        public RestaurantsController(IRestaurantRepository restaurantRepository, IReviewRepository reviewRepository)
        {
            _restaurantRepository = restaurantRepository;
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public Restaurant Get(int id)
        {
            return _restaurantRepository.GetById(id);
        }

        [HttpGet]
        public IEnumerable<Restaurant> GetBy([Required] string city)
        {
            return _restaurantRepository.Get(city);
        }

        [HttpGet]
        [Route("{id}/Reviews")]
        public IEnumerable<Review> GetReviews(int id)
        {
            return _reviewRepository.Get(restaurantId: id);
        }

        [HttpPost]
        public int Post(Restaurant restaurant)
        {
            return _restaurantRepository.Insert(restaurant);
        }

        [HttpPut]
        [Route("{id}")]
        public int Put(int id, Restaurant restaurant)
        {
            return _restaurantRepository.Update(id, restaurant);
        }
    }
}
