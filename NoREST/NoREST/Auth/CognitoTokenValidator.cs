namespace NoREST.Auth
{
    public class CognitoTokenValidator
    {
        private readonly ICognitoPoolInfo _cognitoConfig;
        private readonly IKeyIdHandler _keyIdHandler;

        public CognitoTokenValidator(IKeyIdHandler keyIdHandler, ICognitoPoolInfo cognitoConfig)
        {
            _keyIdHandler = keyIdHandler;
            _cognitoConfig = cognitoConfig;
        }

        public async Task<string> AuthorizeToken(JwtModel tokenModel, DateTime now)
        {
            try
            {
                var isValidToken = true;

                isValidToken &= MatchesIssuer(tokenModel.Issuer);
                isValidToken &= _cognitoConfig.ClientIds.Contains(tokenModel.Subject);
                isValidToken &= await _keyIdHandler.HasMatchingKeyId(tokenModel.Kid, BuildWellKnownConfigurationUrl()).ConfigureAwait(false);

                if (!isValidToken)
                {
                    return "Unauthorized";
                }

                isValidToken &= tokenModel.ValidTo > now;

                if (!isValidToken)
                {
                    return "Token expired";
                }

                isValidToken &= tokenModel.Claims?.Any(c => c.Type == "token_use" && c.Value == "access") == true;
                
                if (!isValidToken)
                {
                    return "Token claims not granted at this resource";
                }
            }
            catch (Exception)
            {
                return "Unexpected error validating token";
            }

            return null;
        }

        private string BuildWellKnownConfigurationUrl() => CognitoPoolAddressBuilder.GetWellKnownConfigurationUrl(_cognitoConfig);

        private bool MatchesIssuer(string issuer) => issuer.Equals(
            CognitoPoolAddressBuilder.GetCognitoUserPoolBaseAddress(_cognitoConfig), StringComparison.InvariantCultureIgnoreCase
            );
    }
}

