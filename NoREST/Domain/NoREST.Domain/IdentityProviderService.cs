using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using NoREST.Models;
using NoREST.Models.DomainModels;
using System.Net.Http.Json;
using System.Text;

namespace NoREST.Domain
{
    public class IdentityProviderService : IIdentityProviderService
    {
        private readonly IAmazonCognitoIdentityProvider _client;
        private readonly ICognitoPoolInfo _poolInfo;

        public IdentityProviderService(IAmazonCognitoIdentityProvider client, ICognitoPoolInfo poolInfo)
        {
            _client = client;
            _poolInfo = poolInfo;
        }

        public async Task CreateUser(UserCreation user)
        {
            var request = new SignUpRequest
            {
                ClientId = _poolInfo.IntegrationClientIds.First(),
                Username = user.Username,
                Password = user.Password
            };

            var emailAttribute = new AttributeType
            {
                Name = "email",
                Value = user.Email
            };

            request.UserAttributes.Add(emailAttribute);

            var response = _client.SignUpAsync(request);
            ;
        }

        public async Task<string> GetTokenFromAuthorizationCode(string authorizationCode, string redirectUrl)
        {
            using (var httpClient = new HttpClient())
            {
                var clientId = _poolInfo.IntegrationClientIds.First();/* Just get the first for now-- possibly adding more clients later */
                var authHeader = Convert.ToBase64String(Encoding.Default.GetBytes($"{clientId}:{_poolInfo.Secret}"));
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"client_id", clientId },
                    {"grant_type", "authorization_code" },
                    {"code", authorizationCode },
                    {"redirect_uri", redirectUrl }
                });

                content.Headers.Add("Authorization", "Basic " + authHeader);

                var response = await httpClient.PostAsync("https://norest.auth.us-east-1.amazoncognito.com/oauth2/token", content);
                var result = await response.Content.ReadFromJsonAsync<CognitoTokenResult>();
                return result?.access_token;
            }
        }


        private class CognitoTokenResult
        {
            public string access_token { get; set; }
        }
    }
}
