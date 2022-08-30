using Microsoft.AspNetCore.Mvc;
using NoREST.Api.Auth;
using NoREST.Domain;
using NoREST.Models.DomainModels;
using NoREST.Models.ViewModels.Creation;

namespace NoREST.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserLogic _userLogic;
        private readonly IRestaurantLogic _restaurantLogic;
        private readonly IAuthService _authService;
        private readonly ILogger<UserController> _logger;
        private readonly IBanManager _banManager;

        public UserController(IUserLogic userLogic, IAuthService authService, IRestaurantLogic restaurantLogic, ILogger<UserController> logger, IBanManager banManager)
        {
            _userLogic = userLogic;
            _authService = authService;
            _restaurantLogic = restaurantLogic;
            _logger = logger;
            _banManager = banManager;
        }

        [HttpPost("")]
        [CognitoAuthorization]
        public async Task<IActionResult> Create([FromBody] UserCreation userCreation)
        {
            try
            {
                var response = await _userLogic.CreateUser(userCreation);
                if (response.IsSuccess)
                {
                    return Created($"user/{response.Value.Id}", response.Value);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, response.ErrorMessage);
                }

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                var user = await _userLogic.GetUser(id);
                return Ok(user);
            }
            catch (Exception)
            {
                return NotFound();
            }            
        }

        [HttpGet("{id}/reviews")]
        public async Task<IActionResult> GetReviewsByUser([FromRoute] int id)
        {
            try
            {
                var reviews = await _restaurantLogic.SearchReviews(new ReviewSearchFilter { UserId = id });
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error fetching reviews for user with id {id}.");
                return StatusCode(500);
            }
        }


        [HttpPost("{userId}/restaurant/{restaurantId}/ban")]
        [CognitoAuthorization]
        public async Task<IActionResult> BanUserFromRestaurant([FromRoute] int userId, [FromRoute] int restaurantId, [FromBody] string reason)
        {
            try
            {
                var currentUser = _authService.GetCurrentlyAuthenticatedUser();
                var result = await _banManager.BanUserFromRestaurant(userId, restaurantId, reason, currentUser);

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
                _logger.LogError(ex, $"Unexpected error creating Ban for userId {userId} and restaurant Id {restaurantId}.");
                return StatusCode(500);
            }
        }
    }
}
