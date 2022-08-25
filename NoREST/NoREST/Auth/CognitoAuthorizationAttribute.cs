using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.JsonWebTokens;
using NoREST.Domain;

namespace NoREST.Api.Auth
{
    public class CognitoAuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public CognitoAuthorizationAttribute()
        {

        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var config = context.HttpContext.RequestServices.GetService<IConfiguration>();
                var tokenHandler = new JsonWebTokenHandler();
                var token = tokenHandler.ReadJsonWebToken(RemoveBearer(context.HttpContext.Request.Headers["Authorization"].First()));
                var cognitoTokenValidator = context.HttpContext.RequestServices.GetService<ICognitoTokenValidator>();
                var tokenModel = new JwtModel(token);
                var (error, user) = await cognitoTokenValidator.ValidateToken(tokenModel, DateTime.UtcNow);

                if (!string.IsNullOrWhiteSpace(error))
                {
                    context.Result = new JsonResult(new { message = error }) { StatusCode = StatusCodes.Status401Unauthorized };
                }

                context.HttpContext.Items[AuthService.UserKey] = user;
            }
            catch (Exception ex)
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

