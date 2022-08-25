using NoREST.Models;

namespace NoREST.Domain
{
    public interface IIdentityProviderService
    {
        Task CreateUser(UserCreation user);
        Task<string> GetTokenFromAuthorizationCode(string authorizationCode, string redirectUrl);
    }
}