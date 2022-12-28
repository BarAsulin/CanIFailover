using System;
using System.Collections.Generic;
using System.Text;

namespace CanIFailover.SQLObjects
{
    public class VMDataStorageObject
    {
        public Guid VpgId { get; set; }
        public string VmIdentifierInternalVmName { get; set; }
        public string RecoveryHostInternalHostName { get; set; }
    }
}