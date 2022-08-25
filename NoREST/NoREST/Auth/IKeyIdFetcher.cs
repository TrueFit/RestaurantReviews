namespace NoREST.Api.Auth
{
    public interface IKeyIdFetcher
    {
        Task<IEnumerable<string>> FetchKeyId(string wellKnownConfigurationUrl);
    }
}

