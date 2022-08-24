namespace NoREST.Auth
{
    public class CognitoPoolInfo : ICognitoPoolInfo
    {
        public List<string> ClientIds { get; } = new List<string>();

        public string PoolId { get; }

        public string Region { get; }

        public string[] Scopes { get; }

        public CognitoPoolInfo(IConfiguration config)
        {
            PoolId = config["Cognito:PoolId"];
            Region = config["Cognito:Region"];
            Scopes = config["Cognito:Scopes"].Split(',');
            ClientIds.Add(config["Cognito:Secret"]);
        }
    }
}

