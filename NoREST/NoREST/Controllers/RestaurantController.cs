using Microsoft.AspNetCore.Mvc;
using NoREST.Api.Auth;
using NoREST.Domain;
using NoREST.Models;
using NoREST.Models.DomainModels;

namespace NoREST.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRestaurantLogic _restaurantLogic;
        private readonly ILogger<RestaurantController> _logger;

        public RestaurantController(IRestaurantLogic restaurantLogic, ILogger<RestaurantController> logger, IAuthService authService)
        {
            _restaurantLogic = restaurantLogic;
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("")]
        [CognitoAuthorization]
        public async Task<IActionResult> Create([FromBody] RestaurantCreation restaurantCreation)
        {
            try
            {
                var user = _authService.GetCurrentlyAuthenticatedUser();
                var restaurant = await _restaurantLogic.Create(restaurantCreation, user);
                if (restaurant != null)
                {
                    return Created($"restaurant/{restaurant.Id}", restaurant);
                }
                else
                {
                    return Conflict();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error");
                return StatusCode(500);
            }
        }

        [HttpPost("review")]
        [CognitoAuthorization]
        public async Task<IActionResult> Review([FromBody] ReviewCreation reviewCreation)
        {
            try
            {
                var user = _authService.GetCurrentlyAuthenticatedUser();
                var result = await _restaurantLogic.CreateReview(reviewCreation, user);
                if (result.IsSuccess)
                {
                    return Created($"restaurant/review/{result.Value.Id}", result.Value);
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(result.ErrorMessage);
                }
                else
                {
                    return StatusCode((int)result.StatusCode, result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error");
                return StatusCode(500);
            }
        }

        [HttpDelete("review/{id}")]
        [CognitoAuthorization]
        public async Task<IActionResult> DeleteReview([FromRoute] int reviewId)
        {
            try
            {
                var user = _authService.GetCurrentlyAuthenticatedUser();
                var result = await _restaurantLogic.DeleteReview(reviewId, user);

                if (result.IsSuccess)
                {
                    return Ok();
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return Unauthorized(result.ErrorMessage);
                }
                else
                {
                    return StatusCode((int)result.StatusCode, result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error");
                return StatusCode(500);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchRestaurant([FromQuery] string city)
        {
            var filter = new RestaurantSearchFilter
            {
                City = city
            };

            try
            {
                var result = await _restaurantLogic.SearchRestaurants(filter);
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error while searching with {filter}.");
                return StatusCode(500);
            }
        }

        [HttpGet("reviews/search")]
        public async Task<IActionResult> GetReviews([FromQuery] int? restaurantId, string restaurantName, int? userId, bool? isActive)
        {
            var filter = new ReviewSearchFilter
            {
                IsActive = isActive,
                UserId = userId,
                RestaurantId = restaurantId,
                RestaurantName = restaurantName
            };

            try
            {
                var result = await _restaurantLogic.SearchReviews(filter);
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error while searching with {filter}.");
                return StatusCode(500);
            }
        }
    }


}
