namespace NoREST.Auth
{
    public interface IKeyIdFetcher
    {
        Task<IEnumerable<string>> FetchKeyId(string wellKnownConfigurationUrl);
    }
}

