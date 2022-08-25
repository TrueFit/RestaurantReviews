using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace NoREST.Api.Auth
{
    public class KeyIdFetcher : IKeyIdFetcher
    {
        public async Task<IEnumerable<string>> FetchKeyId(string wellKnownConfigurationUrl)
        {
            using (var client = new HttpClient())
            {
                var wellKnowConfiguration = await client.GetAsync(wellKnownConfigurationUrl);
                var configResponse = await wellKnowConfiguration.Content.ReadAsStringAsync();
                if (configResponse == null)
                    throw new ApplicationException($"Failed to fetch wellKnownConfiguration from {wellKnownConfigurationUrl}");
                var configObject = JsonSerializer
                    .Deserialize<WellKnownConfiguration>(configResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (configObject == null)
                    throw new ApplicationException($"WellKnownConfiguration from {wellKnownConfigurationUrl} was in unexpected Format");

                return configObject.Keys.Select(k => k.Kid);
            }
        }

        private class WellKnownConfiguration
        {
            public WellKnownConfigurationKey[] Keys { get; set; }
        }

        private class WellKnownConfigurationKey
        {
            public string Kid { get; set; }
        }
    }
}

