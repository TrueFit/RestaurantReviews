using NoREST.Models.DomainModels;

namespace NoREST.Api.Auth
{
    public class CognitoPoolInfo : ICognitoPoolInfo
    {
        public List<string> IntegrationClientIds { get; } = new List<string>();

        public string PoolId { get; }

        public string Region { get; }

        public string[] Scopes { get; }
        public string Secret { get; }

        public CognitoPoolInfo(IConfiguration config)
        {
            PoolId = config["Cognito:PoolId"];
            Region = config["Cognito:Region"];
            Scopes = config["Cognito:Scopes"].Split(',');
            IntegrationClientIds.Add(config["Cognito:ClientId"]);
        }
    }
}

