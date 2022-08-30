using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Logging;
using NoREST.Models.DomainModels;
using NoREST.Models.ViewModels.Creation;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace NoREST.Domain
{
    public class IdentityProviderService : IIdentityProviderService
    {
        private readonly IAmazonCognitoIdentityProvider _client;
        private readonly ICognitoPoolInfo _poolInfo;
        private readonly ILogger<IIdentityProviderService> _logger;

        public IdentityProviderService(IAmazonCognitoIdentityProvider client, ICognitoPoolInfo poolInfo, ILogger<IIdentityProviderService> logger)
        {
            _client = client;
            _poolInfo = poolInfo;
            _logger = logger;
        }

        public async Task<UserCreationResponse> CreateUser(UserCreation user)
        {
            var clientId = _poolInfo.ClientId;
            string secretHash = _poolInfo.ComputeSecretHash(user.Username);

            var request = new SignUpRequest
            {
                ClientId = clientId,
                Username = user.Username,
                Password = user.Password,
                SecretHash = secretHash                
            };

            var emailAttribute = new AttributeType
            {
                Name = "email",
                Value = user.Email
            };

            request.UserAttributes.Add(emailAttribute);

            try
            {
                var response = await _client.SignUpAsync(request);
                if (response != null && response.HttpStatusCode == HttpStatusCode.OK)
                {
                    var confirmation = await _client.AdminConfirmSignUpAsync(new AdminConfirmSignUpRequest
                    {
                        UserPoolId = _poolInfo.PoolId,
                        Username = user.Username
                    });
                    return new UserCreationResponse
                    {
                        IsSuccess = true,
                        IdentityProviderId = response.UserSub
                    };
                }
                else 
                {
                    return new UserCreationResponse
                    {
                        IsSuccess = false,
                        Error = "Unable to create user. Unsuccessful response from Identity Provider"
                    };
                }
            }
            catch (AmazonCognitoIdentityProviderException awsex)
            {
                _logger.LogInformation(awsex.Message);
                return new UserCreationResponse
                {
                    IsSuccess = false,
                    Error = awsex.Message
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error attempting to create user");
                return new UserCreationResponse
                {
                    Error = "Unexpected error",
                    IsSuccess = false
                };
            }
        }

        public async Task<string> GetTokenFromAuthorizationCode(string authorizationCode, string redirectUrl)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var clientId = _poolInfo.UiClientId;
                    var authHeader = Convert.ToBase64String(Encoding.Default.GetBytes($"{clientId}:{_poolInfo.ClientSecret}"));
                    var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"client_id", clientId },
                    {"grant_type", "authorization_code" },
                    {"code", authorizationCode },
                    {"redirect_uri", redirectUrl }
                });

                    content.Headers.TryAddWithoutValidation("Authorization", "Basic " + authHeader);

                    var response = await httpClient.PostAsync("https://norest.auth.us-east-1.amazoncognito.com/oauth2/token", content);
                    var result = await response.Content.ReadFromJsonAsync<CognitoTokenResult>();
                    return result?.access_token;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public async Task<bool> RemoveUser(UserCreation user)
        {
            try
            {
                await _client.AdminDeleteUserAsync(new AdminDeleteUserRequest() { Username = user.Username, UserPoolId = _poolInfo.PoolId });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        private class CognitoTokenResult
        {
            public string access_token { get; set; }
        }
    }
}
