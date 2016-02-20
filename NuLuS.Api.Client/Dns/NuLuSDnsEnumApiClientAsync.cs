using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Heijden.DNS;

namespace NuLuS.Api.Client.Dns
{
    internal sealed class NuLuSDnsEnumApiClientAsync : INuLuSApiClientAsync
    {
        private static readonly string Address;

        private readonly Lazy<Regex> _resultRegex = new Lazy<Regex>(() => new Regex(@"!\^\.\*\$!sip:\+(\d+)\@ims\.mnc(\d+)\.mcc(\d+)\.3gppnetwork\.org", RegexOptions.Singleline | RegexOptions.Compiled));
        private readonly Lazy<Regex> _resultValueRegex = new Lazy<Regex>(() => new Regex(@"(.*)=(.*)", RegexOptions.Singleline | RegexOptions.Compiled));

        static NuLuSDnsEnumApiClientAsync()
        {
            Address = ConfigurationManager.AppSettings["NuLuS.Dns.Api.Address"];
            if (string.IsNullOrEmpty(Address))
            {
                throw new SettingsPropertyNotFoundException("App setting \"NuLuS.Dns.Api.Address\" not set!");
            }
        }

#pragma warning disable 1998
        public async Task<LookupResponse[]> LookupAsync(string product, params string[] phoneNumbers)
#pragma warning restore 1998
        {
            if (phoneNumbers.Length != 1)
            {
                throw new NotImplementedException(); //TODO: Import modified Heijden
            }

            var resolver = new Resolver(Address)
            {
                TransportType = TransportType.Udp,
                UseCache = false,
                TimeOut = 20,
                Retries = 1
            };

            var strName = Resolver.GetArpaFromEnum(phoneNumbers.Single());
            if (strName.EndsWith(".e164.arpa.") && !string.IsNullOrEmpty(product))
            {
                strName = strName.Insert(strName.Length - ".e164.arpa.".Length, "." + product.ToLower());
            }
            var response = resolver.Query(strName, QType.NAPTR, QClass.IN);
            var naptrRecord = response.Answers.Select(answer => answer.RECORD).OfType<RecordNAPTR>().FirstOrDefault();

            // ReSharper disable once PossibleNullReferenceException
            var responseParts = naptrRecord.REGEXP.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (responseParts.Length == 0) return null;
            // ReSharper disable once PossibleNullReferenceException
            var match = _resultRegex.Value.Match(responseParts[0]);

            var lookupResponse = new LookupResponse
            {
                InputPhoneNumber = phoneNumbers.Single(),
                MSISDN = match.Success ? match.Groups[1].Value : null,
                MobileCountryCode = match.Success ? match.Groups[3].Value : null,
                MobileNetworkCode = match.Success ? match.Groups[2].Value : null
            };

            for (var i = 1; i < responseParts.Length; i++)
            {
                var fieldMatch = _resultValueRegex.Value.Match(responseParts[i]);
                if (!fieldMatch.Success) continue;

                switch (fieldMatch.Groups[1].Value)
                {
                    case "npdi":
                        lookupResponse.NPDI = Convert.ToBoolean(int.Parse(fieldMatch.Groups[2].Value));
                        break;
                    case "price":
                        lookupResponse.Price = decimal.Parse(fieldMatch.Groups[2].Value, CultureInfo.InvariantCulture.NumberFormat);
                        break;

                    case "nrhmnc":
                        lookupResponse.PortedSourceMobileNetworkCode = fieldMatch.Groups[2].Value;
                        break;
                    case "imsi":
                        lookupResponse.IMSI = fieldMatch.Groups[2].Value;
                        break;
                    case "msc":
                        lookupResponse.MSC = fieldMatch.Groups[2].Value;
                        break;
                    case "isroaming":
                        lookupResponse.IsRoaming = Convert.ToBoolean(int.Parse(fieldMatch.Groups[2].Value));
                        break;
                    case "rmcc":
                        lookupResponse.RoamingMobileCountryCode = fieldMatch.Groups[2].Value;
                        break;
                    case "isabsent":
                        lookupResponse.IsAbsent = Convert.ToBoolean(int.Parse(fieldMatch.Groups[2].Value));
                        break;
                }
            }

            return new[] { lookupResponse }; //TODO: Make async response from Task.Factory.FromAsync(resolver.BeginResolve, resolver.EndResolve)
            // https://blog.justjuzzy.com/2012/10/turn-iasyncresult-code-into-await-keyword/
        }
    }
}
