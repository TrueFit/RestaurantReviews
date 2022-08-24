using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoREST.ViewModels;

namespace NoREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// For the sake of expediency (avoiding user sign-up etc.)
        /// let's pretend the token returned here is for the specific user        /// 
        /// </summary>
        /// <param name="userCreds"></param>
        /// <returns>You're actually getting an app integration token</returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Authenticate([FromBody] UserLogin userCreds)
        {
            using (var httpClient = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"client_id", "5qg4one8ve2o7024st8dks8ejq"},
                    {"client_secret", "19360lo1p225u36p1d6aboal4rsnjo6ohhsf9jia5fcj4mtob9ui" },
                    {"grant_type", "client_credentials" }
                });
                var response = await httpClient.PostAsync("https://norest.auth.us-east-1.amazoncognito.com/oauth2/token", content);
                var result = await response.Content.ReadFromJsonAsync<CognitoTokenResult>();
                return Ok(result?.access_token);
            }
        }

        private class CognitoTokenResult
        {
            public string access_token { get; set; }
        }
    }
}
