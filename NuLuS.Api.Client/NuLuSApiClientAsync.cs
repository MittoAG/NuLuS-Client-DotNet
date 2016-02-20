using System;
using System.Configuration;
using System.Threading.Tasks;
using NuLuS.Api.Client.Dns;
using NuLuS.Api.Client.Http;

namespace NuLuS.Api.Client
{
    public sealed class NuLuSApiClientAsync : INuLuSApiClientAsync
    {
        private static readonly QueryChannel DefaultChannel;

        static NuLuSApiClientAsync()
        {
            var channelConfigString = ConfigurationManager.AppSettings["NuLuS.Api.Channel"];
            if (!string.IsNullOrEmpty(channelConfigString))
            {
                Enum.TryParse(channelConfigString, out DefaultChannel);
            }
        }

        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<NuLuSApiClientAsync> _instance = new Lazy<NuLuSApiClientAsync>(() => new NuLuSApiClientAsync());
        public static NuLuSApiClientAsync Instance => _instance.Value;
        
        private readonly INuLuSApiClientAsync _client;

        public NuLuSApiClientAsync()
            : this(DefaultChannel)
        {
        }

        public NuLuSApiClientAsync(QueryChannel queryChannel)
        {
            switch (queryChannel)
            {
                case QueryChannel.DnsEnum:
                    _client = new NuLuSDnsEnumApiClientAsync();
                    break;
                case QueryChannel.HttpRest:
                    _client = new NuLuSHttpRestApiClientAsync();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(queryChannel));
            }
        }

        public Task<LookupResponse[]> LookupAsync(string product, params string[] phoneNumbers)
        {
            return _client.LookupAsync(product, phoneNumbers);
        }
    }
}
