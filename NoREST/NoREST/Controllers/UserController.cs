using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoREST.Auth;
using NoREST.ViewModels;

namespace NoREST.Controllers
{
    [ApiController]
    [CognitoAuthorization]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("")]
        public IActionResult Create([FromBody] UserCreation userCreation)
        {
            return Created("user/1", 1);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            return Ok(new User() { Id = id, UserName = "Me"});
        }
    }
}
