namespace NoREST.Api.Auth
{
    public interface IKeyIdHandler
    {
        Task<bool> HasMatchingKeyId(string kid, string wellKnownConfigurationUrl);
    }
}

