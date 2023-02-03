using Microsoft.AspNetCore.Mvc;
using RestaurantReviews.Domain.AggregatesModel.ReviewAggregate;
using userAg = RestaurantReviews.Domain.AggregatesModel.UserAggregate;

namespace RestaurantReviews.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly userAg.IUserRepository _userRepository;

        public UsersController(IReviewRepository reviewRepository , userAg.IUserRepository userRepository)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public userAg.User Get(int id)
        {
            return _userRepository.GetById(id);
        }

        [HttpGet]
        [Route("{id}/Reviews")]
        public IEnumerable<Review> GetReviews(int id)
        {
            return _reviewRepository.Get(userId: id);
        }

        [HttpPost]
        public int Post([FromBody] userAg.User user)
        {
            return _userRepository.Insert(user);
        }

        [HttpPut]
        [Route("{id}")]
        public int Put(int id, [FromBody] userAg.User user)
        {
            return _userRepository.Update(id, user);
        }
    }
}
