using System;
using System.Collections.Generic;
using System.Text;

namespace CanIFailover
{
    internal class DBConstantQuerys
    {
        public static readonly string GetVMResourceInfoStorageObjectLast5Days = "select [VmIdInternalVmName],[VpgName],[NumberOfvCpu],[CpuLimitInMhz],[CpuReservedInMhz],[CpuUsedInMhz],[MemoryInMB],[MemoryReservedInMB],[MemoryLimitInMB],[ConsumedHostMemoryInMB],[Timestamp] from [VmResourcesInfoStorageObject] where DATEDIFF(day,timestamp,GETDATE()) between 0 and 5";
        public static readonly string GetVCenterSettingStorageObjectHostname = "select [Username],[Hostname] from [VCenterSettingsStorageObject]";
        public static readonly string GetVMDataStorageObject = "select [ProtectionGroupId],[VmIdentifierServerIdentifierServerGuid],[RecoveryHostInternalHostName] from [VPG_VMDataStorageObject] where [RecoveryHostServerIdentifierServerGuid] = ";
        public static readonly string GetvCenterGuid = "select top 1 [HostIdentifierServerIdentifierServerGuid] from [VraInfoStorageObject]";
        public static readonly string GetVRAsInternalHostname = "select [HostIdentifierInternalHostName],[VraInternalIdentifierIdentifier] from [VraInfoStorageObject]";
        public static readonly string GetIdentifierMapperHosts = "select[InternalId],[ExternalId] from[IdentifierMapperStorageObject] where[Type] = 'HostSystem'";
    }

}