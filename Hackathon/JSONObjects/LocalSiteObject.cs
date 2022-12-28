using System;
using System.Collections.Generic;
using System.Text;

namespace CanIFailover.JSONObjects
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Link
    {
        public object rel { get; set; }
        public string href { get; set; }
        public string type { get; set; }
        public string identifier { get; set; }
    }
    public class LocalSiteObject
    {
        public static readonly string Uri = "/v1/localsite";
        public Link Link { get; set; }
        public string SiteIdentifier { get; set; }
        public string SiteName { get; set; }
        public string Location { get; set; }
        public string Version { get; set; }
        public string ZvmApiVersion { get; set; }
        public object ContactEmail { get; set; }
        public string ContactName { get; set; }
        public object ContactPhone { get; set; }
        public bool IsReplicationToSelfEnabled { get; set; }
        public int UtcOffsetInMinutes { get; set; }
        public string SiteType { get; set; }
        public string IpAddress { get; set; }
        public double BandwidthThrottlingInMBs { get; set; }
        public string SiteTypeVersion { get; set; }
        public bool IsLoginBannerEnabled { get; set; }
        public string LoginBanner { get; set; }
    }
}
