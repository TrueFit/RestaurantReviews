using NoREST.Models;
using NoREST.Models.DomainModels;

namespace NoREST.Domain
{
    public interface IIdentityProviderService
    {
        Task<bool> RemoveUser(UserCreation user);
        Task<UserCreationResponse> CreateUser(UserCreation user);
        Task<string> GetTokenFromAuthorizationCode(string authorizationCode, string redirectUrl);
    }
}