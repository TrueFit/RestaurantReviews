using NoREST.Models;

namespace NoREST.Api.Auth
{
    public interface ICognitoTokenValidator
    {
        Task<(string errorMessage, UserProfile userProfile)> ValidateToken(JwtModel tokenModel, DateTime now);
    }
}