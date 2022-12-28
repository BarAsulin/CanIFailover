using System;
using System.Collections.Generic;
using System.Text;

namespace CanIFailover.VCObjects
{
    class EsxHost
    {
        public EsxHost(string externalId, string hostname, long totalMemoryInBytes, long usedMemoryInMBytes, int totalCpuInMhz, int usedCpuInMhz, int totalCores, int usedCores)
        {
            HostExternalId = externalId;
            HostName = hostname;
            TotalMemoryInBytes = totalMemoryInBytes;
            UsedMemoryInMBytes = usedMemoryInMBytes;
            TotalCpuInMhz = totalCpuInMhz;
            UsedCpuInMhz = usedCpuInMhz;
            TotalCores = totalCores;
            UsedCores = usedCores;

        }
        public string HostInternalId { get; set; }
        public string HostExternalId { get; private set; }
        public string HostName { get; private set; }
        public long TotalMemoryInBytes { get; private set; }
        public long UsedMemoryInMBytes { get; private set; }
        public int TotalCpuInMhz { get; private set; }
        public int UsedCpuInMhz { get; private set; }
        public int TotalCores { get; private set; }
        public int UsedCores { get; private set; }
    }
}