using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoREST.Api.Auth;
using NoREST.Domain;
using NoREST.Models;
using System.Text;

namespace NoREST.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityProviderService _identityProviderService;

        public AuthController(IIdentityProviderService identityProviderService)
        {
            _identityProviderService = identityProviderService;
        }


        /// <summary>
        /// For the sake of expediency (avoiding user sign-up etc.)
        /// let's pretend the token returned here is for the specific user 
        /// </summary>
        /// <param name="userCreds"></param>
        /// <returns>You're actually getting an app integration token</returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Authenticate([FromBody] string authorizationCode, [FromQuery] string redirectUri)
        {
            var token = await _identityProviderService.GetTokenFromAuthorizationCode(authorizationCode, redirectUri);
            if (string.IsNullOrWhiteSpace(token)) return Unauthorized();
            return Ok(token);
        }

    }
}
