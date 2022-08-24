namespace NoREST.Auth
{
    //Maybe we don't needthis?
    public interface IKeyIdHandler
    {
        Task<bool> HasMatchingKeyId(string kid, string wellKnownConfigurationUrl);
    }
}

