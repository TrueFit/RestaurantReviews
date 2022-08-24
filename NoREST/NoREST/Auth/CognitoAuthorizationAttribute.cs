using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.JsonWebTokens;

namespace NoREST.Auth
{
    public class CognitoAuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var config = context.HttpContext.RequestServices.GetService<IConfiguration>();
                var tokenHandler = new JsonWebTokenHandler();
                var token = tokenHandler.ReadJsonWebToken(RemoveBearer(context.HttpContext.Request.Headers["Authorization"].First()));
                var cognitoTokenValidator = new CognitoTokenValidator(new KeyIdHandler(new KeyIdFetcher()), new CognitoPoolInfo(context.HttpContext.RequestServices.GetService<IConfiguration>()));
                var tokenModel = new JwtModel(token);
                var error = await cognitoTokenValidator.AuthorizeToken(tokenModel, DateTime.UtcNow);

                if (!string.IsNullOrWhiteSpace(error))
                {
                    context.Result = new JsonResult(new { message = error }) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
            catch (Exception)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

        }

        private string RemoveBearer(string authHeader)
        {
            return authHeader.Substring("bearer ".Length);
        }
    }
}

