namespace NoREST.Api.Auth
{
    public class KeyIdHandler : IKeyIdHandler
    {
        //ToDo: replace with MemCache...
        private static IEnumerable<string> Kids;
        private readonly IKeyIdFetcher _keyIdFetcher;
        private static readonly ILogger<IKeyIdHandler> _logger;

        public KeyIdHandler(IKeyIdFetcher keyIdFetcher)
        {
            _keyIdFetcher = keyIdFetcher;
        }

        public async Task<bool> HasMatchingKeyId(string kid, string wellKnownConfigurationUrl)
        {
            if (Kids == null)
            {
                await GetKids(wellKnownConfigurationUrl).ConfigureAwait(false);
            }

            return Kids.Contains(kid);
        }

        private async Task GetKids(string wellKnownConfigurationUrl)
        {
            try
            {
                Kids = await _keyIdFetcher.FetchKeyId(wellKnownConfigurationUrl).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not access {wellKnownConfigurationUrl}");
                throw;
            }
        }
    }
}

