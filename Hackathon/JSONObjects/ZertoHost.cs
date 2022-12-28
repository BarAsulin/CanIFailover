using System;
using System.Collections.Generic;
using System.Text;

namespace CanIFailover.JSONObjects
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class AssociatedDatastore
    {
        public string DatastoreName { get; set; }
        public string DatastoreIdentifier { get; set; }
        public object OwningDatastoreCluster { get; set; }
    }
    public class AssociatedNetwork
    {
        public string VirtualizationNetworkName { get; set; }
        public string NetworkIdentifier { get; set; }
    }
    public class ZertoHost
    {
        public static string Uri(string SiteId)
        {
            return "/v1/virtualizationsites/" + SiteId + "/hosts";
        }
        public List<AssociatedDatastore> AssociatedDatastores { get; set; }
        public List<AssociatedNetwork> AssociatedNetworks { get; set; }
        public string HostIdentifier { get; set; }
        public string VirtualizationHostName { get; set; }
    }
}
