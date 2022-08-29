namespace NoREST.Models.DomainModels
{
    public interface ICognitoPoolInfo
    {
        string ClientId { get; }
        string PoolId { get; }
        string Region { get; }
        string[] Scopes { get; }
        string ClientSecret { get; }
        string UiClientId { get; }

        string ComputeSecretHash(string username);
    }
}

