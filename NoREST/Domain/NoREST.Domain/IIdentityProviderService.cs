using NoREST.Models.DomainModels;
using NoREST.Models.ViewModels.Creation;

namespace NoREST.Domain
{
    public interface IIdentityProviderService
    {
        Task<bool> RemoveUser(UserCreation user);
        Task<UserCreationResponse> CreateUser(UserCreation user);
        Task<string> GetTokenFromAuthorizationCode(string authorizationCode, string redirectUrl);
    }
}