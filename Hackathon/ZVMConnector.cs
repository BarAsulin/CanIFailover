using System;
using System.Collections.Generic;
using System.Text;

namespace CanIFailover
{
    internal class ZVMConnector
    {
    }
}
// query to get VMs recovery hosts:
// select [ProtectionGroupId],[VmIdentifierInternalVmName],[RecoveryHostInternalHostName] where [RecoveryHostServerIdentifierServerGuid] = <VCGUID>
// select * from VPG_VMSETTINGSstorageobject where ReplicationDestinationComputeResourceServerIdentifierServerGuid = '{A7233627-711A-44D3-84D8-82B234AC5158}' and VMDATASTORAGEObjeCTID is not null
// select * from vpg_vmdatastorageobject where recoveryhostinternalhostname is not null and recoveryhostserveridentifierserverguid = '{A7233627-711A-44D3-84D8-82B234AC5158}'
//
//
//
/* will provide vm to recovery host identification (internal ids)
 * select protectiongroupid,vmidentifierinternalvmname,recoveryhostinternalhostname from vpg_vmdatastorageobject where recoveryhostinternalhostname is not null and recoveryhostserveridentifierserverguid = '{A7233627-711A-44D3-84D8-82B234AC5158}'
 * 
 * 
 * ------
 * to select 30 days of content:
 * select timestamp,vmname,vpgname from vmresourcesinfostorageobject where vmname = 'ubuntu18' and DATEDIFF(day,timestamp,GETDATE()) between 0 and 30 
 * 
 * select timestamp,vmname,vpgname from vmresourcesinfostorageobject where vmname = 'ubuntu18' and timestamp >= CURRENT_TIMESTAMP -30
 * 
 */