namespace NoREST.Auth
{
    public interface ICognitoPoolInfo
    {
        List<string> ClientIds { get; }
        string PoolId { get; }
        string Region { get; }
        string[] Scopes { get; }
    }
}

