using System.Collections.Generic;

namespace NuLuS.Api.Client
{
    public sealed class LookupRequest
    {
        public IEnumerable<string> PhoneNumbers { get; set; }
    }
}
