namespace NoREST.Models.DomainModels
{
    public interface ICognitoPoolInfo
    {
        List<string> IntegrationClientIds { get; }
        string PoolId { get; }
        string Region { get; }
        string[] Scopes { get; }
        string Secret { get; }
    }
}

