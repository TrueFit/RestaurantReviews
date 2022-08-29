using NoREST.Models.DomainModels;
using System.Text;

namespace NoREST.Api.Auth
{
    public class CognitoPoolInfo : ICognitoPoolInfo
    {
        public string ClientId { get; }

        public string PoolId { get; }

        public string Region { get; }

        public string[] Scopes { get; }
        public string ClientSecret { get; }
        public string UiClientId { get; }

        public CognitoPoolInfo(IConfiguration config)
        {
            PoolId = config["Cognito:PoolId"];
            Region = config["Cognito:Region"];
            Scopes = config["Cognito:Scopes"]?.Split(',');
            ClientId = config["Cognito:ClientId"];
            UiClientId = config["Cognito:UiClientId"];
            ClientSecret = config["Cognito:ClientSecret"];
        }

        public string ComputeSecretHash(string username)
        {
            var clientId = ClientId;
            var dataString = username + clientId;
            var data = Encoding.Default.GetBytes(dataString);
            var key = Encoding.Default.GetBytes(ClientSecret);
            using (var shaAlg = new System.Security.Cryptography.HMACSHA256(key))
            {
                return Convert.ToBase64String(shaAlg.ComputeHash(data));
            }
        }
    }
}

