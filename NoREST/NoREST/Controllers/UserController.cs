using Microsoft.AspNetCore.Mvc;
using NoREST.Api.Auth;
using NoREST.Domain;
using NoREST.Models;

namespace NoREST.Api.Controllers
{
    [ApiController]
    [CognitoAuthorization]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserLogic _userLogic;

        public UserController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] UserCreation userCreation)
        {
            var createdUser = await _userLogic.Create(userCreation);
            return Created($"user/{createdUser.Id}", createdUser);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var user = await _userLogic.GetUser(id);
            return Ok(user);
        }
    }
}
