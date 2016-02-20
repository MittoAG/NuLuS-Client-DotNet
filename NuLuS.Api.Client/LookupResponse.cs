using System;

namespace NuLuS.Api.Client
{
    public class LookupResponse
    {
        public string InputPhoneNumber { get; set; }
        
        public string MSISDN { get; set; }
        
        public string MobileCountryCode { get; set; }
        
        public string MobileNetworkCode { get; set; }
        
        public bool? IsPorted { get; set; }
        
        public string PortedSourceMobileNetworkCode { get; set; }
        
        public DateTime ResultDateTime { get; set; }
        
        public string IMSI { get; set; }
        
        public string MSC { get; set; }
        
        public string SMSC { get; set; }
        
        public bool? IsRoaming { get; set; }
        
        public string RoamingMobileCountryCode { get; set; }
        
        public bool? IsAbsent { get; set; }
        
        public bool NPDI { get; set; }
        
        public decimal Price { get; set; }
    }
}
