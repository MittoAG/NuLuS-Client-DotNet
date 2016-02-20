using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NuLuS.Api.Client.Http
{
    internal sealed class NuLuSHttpRestApiClientAsync : INuLuSApiClientAsync
    {
        private static readonly string Url;
        private static readonly Lazy<HttpClient> HttpClient;

        static NuLuSHttpRestApiClientAsync()
        {
            Url = ConfigurationManager.AppSettings["NuLuS.Http.Rest.Api.Url"];
            if (string.IsNullOrEmpty(Url))
            {
                throw new SettingsPropertyNotFoundException("App setting \"NuLuS.Http.Rest.Api.Url\" not set!");
            }
            HttpClient = new Lazy<HttpClient>(() => new HttpClient
            {
                BaseAddress = new Uri(Url, UriKind.Absolute)
            });
        }
        public async Task<LookupResponse[]> LookupAsync(string product, params string[] phoneNumbers)
        {
            var httpResponse = await HttpClient.Value.PostAsync(new Uri(string.IsNullOrEmpty(product) ? "lookup.json" : $"lookup/{product}.json", UriKind.Relative), new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "PhoneNumbers", JsonConvert.SerializeObject(phoneNumbers) }
            }));
            httpResponse.EnsureSuccessStatusCode();
            return await httpResponse.Content.ReadAsAsync<LookupResponse[]>();
        }
    }
}
