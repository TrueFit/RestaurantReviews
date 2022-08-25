using NoREST.Models;

namespace NoREST.Api.Auth
{
    public interface IAuthService
    {
        UserProfile GetCurrentlyAuthenticatedUser();
    }

    public class AuthService : IAuthService
    {
        public const string UserKey = "user";
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public UserProfile GetCurrentlyAuthenticatedUser()
        {
            return _contextAccessor.HttpContext.Items[UserKey] as UserProfile;
        }
    }
}
