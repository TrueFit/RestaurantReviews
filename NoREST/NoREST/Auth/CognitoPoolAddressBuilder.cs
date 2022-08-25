using NoREST.Models.DomainModels;

namespace NoREST.Api.Auth
{
    public static class CognitoPoolAddressBuilder
    {
        public static string GetCognitoUserPoolBaseAddress(ICognitoPoolInfo cognitoPoolInfo) => $"https://cognito-idp.{cognitoPoolInfo.Region}.amazonaws.com/{cognitoPoolInfo.PoolId}";
        public static string GetWellKnownConfigurationUrl(ICognitoPoolInfo cognitoPoolInfo) => $"{GetCognitoUserPoolBaseAddress(cognitoPoolInfo)}/.well-known/jwks.json";
    }
}

