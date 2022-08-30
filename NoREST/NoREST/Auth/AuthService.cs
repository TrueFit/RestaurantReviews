using NoREST.Models.ViewModels.Outgoing;

namespace NoREST.Api.Auth
{
    public interface IAuthService
    {
        UserProfile GetCurrentlyAuthenticatedUser();
    }

    //Keep this internal. HttpContextAccessor should only be used within the context of a web request.
    internal class AuthService : IAuthService
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
