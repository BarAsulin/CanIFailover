using System;
using System.Collections.Generic;
using System.Text;

namespace CanIFailover.SQLObjects
{
    public class VMResourceInfoStorageObject
    {
        public string VmIdInternalVmName { get; set; }
        public string VpgName { get; set; }
        public int NumberOfvCpu { get; set; }
        public int CpuLimitInMhz { get; set; }
        public int CpuReservedInMhz { get; set; }
        public int CpuUsedInMhz { get; set; }
        public int MemoryInMB { get; set; }
        public int MemoryReservedInMB { get; set; }
        public int MemoryLimitInMB { get; set; }
        public int ConsumedHostmemoryInMB { get; set; }
        public DateTime Timestamp { get; set; }
    }
}