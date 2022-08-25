using NoREST.Domain;
using NoREST.Models;
using NoREST.Models.DomainModels;

namespace NoREST.Api.Auth
{
    public class CognitoTokenValidator : ICognitoTokenValidator
    {
        private readonly ICognitoPoolInfo _cognitoConfig;
        private readonly IKeyIdHandler _keyIdHandler;
        private readonly IUserLogic _userLogic;

        public CognitoTokenValidator(IKeyIdHandler keyIdHandler, ICognitoPoolInfo cognitoConfig, IUserLogic userLogic)
        {
            _keyIdHandler = keyIdHandler;
            _cognitoConfig = cognitoConfig;
            _userLogic = userLogic;
        }

        public async Task<(string errorMessage, UserProfile userProfile)> ValidateToken(JwtModel tokenModel, DateTime now)
        {
            try
            {
                var isValidToken = true;

                isValidToken &= MatchesIssuer(tokenModel.Issuer);
                // isValidToken &= _cognitoConfig.ClientIds.Contains(tokenModel.Subject);
                isValidToken &= await _keyIdHandler.HasMatchingKeyId(tokenModel.Kid, BuildWellKnownConfigurationUrl()).ConfigureAwait(false);

                if (!isValidToken)
                {
                    return ("Unauthorized", null);
                }

                //    isValidToken &= tokenModel.ValidTo > now;

                if (!isValidToken)
                {
                    return ("Token expired", null);
                }

                isValidToken &= tokenModel.Claims?.Any(c => c.Type == "token_use" && c.Value == "access") == true;

                if (!isValidToken)
                {
                    return ("Token claims not granted at this resource", null);
                }
            }
            catch (Exception)
            {
                return ("Unexpected error validating token", null);
            }

            var user = await _userLogic.GetUserProfileFromIdentityProviderId(tokenModel.Subject);

            return (null, user);
        }

        private string BuildWellKnownConfigurationUrl() => CognitoPoolAddressBuilder.GetWellKnownConfigurationUrl(_cognitoConfig);

        private bool MatchesIssuer(string issuer) => issuer.Equals(
            CognitoPoolAddressBuilder.GetCognitoUserPoolBaseAddress(_cognitoConfig), StringComparison.InvariantCultureIgnoreCase
            );
    }
}

