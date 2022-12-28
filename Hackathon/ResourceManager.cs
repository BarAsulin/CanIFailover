using CanIFailover.JSONObjects;
using CanIFailover.SQLObjects;
using CanIFailover.VCObjects;
using NetVimClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMware.Vim;

namespace CanIFailover
{
    public class ResourceManager
    {
        //Accessors:
        private VimClientImpl m_vCenterClient; // vC SDK access
        private ZVMApiClient m_zvmApiClient; // ZVM Apis access
        private SQLServer m_sqlServer = new SQLServer(); // ZVM DB access
        // Data lists
        private List<EsxHost> m_esxHosts; // esx hosts and their memory/cpu resoures.
        private List<VMResourceInfoStorageObject> m_VMResourceInfoStorageObjects; // VMs, and their cpu/memory allocations, reservations, and usage.
        private List<VMDataStorageObject> m_VMDataStorageObjects; // VMs, their recovery hosts and VPG id.
        private List<ZertoHost> m_zertoHosts; // Zerto hosts and datastore/network access
        private List<VraInfoHostIdentifierInternalHostName> m_VrasInternalHostName; // hosts with a VRA, their internal id and rcmid for change VM recovery VRA, so we know which target hosts are possible.
        private List<string> m_VpgIdsToValidate = new List<string>(); // vpgs that we were requested to validate.
        public ResourceManager(string username, string password)
        {
            InitializeVCObjects(username, password);
            InitializeSQLObjects(vCenterConnector.GetVCenterGuidAsString(m_vCenterClient));
            m_zvmApiClient = new ZVMApiClient(username, password);
        }
        private void InitializeSQLObjects(string vcenterGuid)
        {
            m_VMDataStorageObjects = m_sqlServer.GetVMDataStorageObjects(vcenterGuid);
            m_VMResourceInfoStorageObjects = m_sqlServer.GetVMResourceInfoStorageObjects();
            m_VrasInternalHostName = m_sqlServer.GetVrasInternalHostname();
            AddInternalIdToEsxHostsList();
            RemoveEsxHostsWithoutVras();
        }
        private void InitializeVCObjects(string username, string password)
        {
            m_vCenterClient = vCenterConnector.ConnectTovCenter(m_sqlServer.GetvCenterIP(), username, password);
            m_esxHosts = vCenterConnector.GetEsxHosts(m_vCenterClient);
        }
        private void AddInternalIdToEsxHostsList()
        {
            List<IdentifierMapperHost> identifierMapperHosts = m_sqlServer.GetIdentifierMapperHosts();
            foreach (EsxHost esxHost in m_esxHosts)
            {
                IdentifierMapperHost host = identifierMapperHosts.Find(x => x.ExternalId == esxHost.HostExternalId);
                esxHost.HostInternalId = host.InternalId;
            }
        }
        public async void InitializeJsonObjects()
        {
            await m_zvmApiClient.AddTokenToHttpClient();
            string siteId = await m_zvmApiClient.GetSiteId();
            m_zertoHosts = await m_zvmApiClient.GetZertoHosts(siteId);
        }
        private void RemoveEsxHostsWithoutVras()
        {
            m_esxHosts = m_esxHosts.Where(x => m_VrasInternalHostName.Any(y => y.HostIdentifierInternalHostName == x.HostInternalId)).ToList();
        }
        // this call assumes that m_VpgIdsToValidate is initialized.
        public void FilterUnnecessaryVMs() // remove VMs which are not in the VPG we are validating, need to remove from both VM lists
        {
            // Need to test if this actually works but overall the idea is to filter all irrelevant VM based on the VPGs we were requested to validate
            m_VMDataStorageObjects = m_VMDataStorageObjects.Where(x => m_VpgIdsToValidate.Any(y => y == x.VpgId.ToString())).ToList();
            // and after this filter, filter the other VM list (m_VMResourceInfoStorageObjects) based on the VMs left in the m_VMDataStorageObjects list.
            m_VMResourceInfoStorageObjects = m_VMResourceInfoStorageObjects.Where(x => m_VMDataStorageObjects.Any(y => y.VmIdentifierInternalVmName == x.VmIdInternalVmName)).ToList();

        }
        // this should be called after FilterUnnecessaryVMs, as we go over the m_VMDataStorageObjects list which should only include VMs we need to validate at this point.
        public void ConsolidateVMsResourceInfoList() // calculate averages for the multiple records and keep only 1 element for each VM
        {
            foreach (VMDataStorageObject VM in m_VMDataStorageObjects)
            {
                var vmRecords = m_VMResourceInfoStorageObjects.FindAll(x => x.VmIdInternalVmName == VM.VmIdentifierInternalVmName);
                VMResourceInfoStorageObject consolidatedObject = new VMResourceInfoStorageObject();
                int i = 0;
                for (; i < vmRecords.Count; i++)
                {
                    // need to go over all the object properties that needs to be summed up and add them to the consolidated object
                    consolidatedObject.ConsumedHostmemoryInMB += vmRecords[i].ConsumedHostmemoryInMB;
                    consolidatedObject.CpuUsedInMhz += vmRecords[i].CpuLimitInMhz;
                    // need to think through which CPU settings is the actual usage and add it over here and divide outside the loop.
                }
                // outside the loop divide all the summed up objects by i.
                consolidatedObject.ConsumedHostmemoryInMB /= i;
                consolidatedObject.CpuUsedInMhz /= i;

                //find the element with the newest date, grab its static settings, such as reservation setting, vCPU and memory allocation.
                VMResourceInfoStorageObject latestTimestamp = vmRecords.OrderBy(x => x.Timestamp).FirstOrDefault();
                consolidatedObject.Timestamp = latestTimestamp.Timestamp;
                consolidatedObject.NumberOfvCpu = latestTimestamp.NumberOfvCpu;
                consolidatedObject.CpuReservedInMhz= latestTimestamp.CpuReservedInMhz;
                consolidatedObject.VpgName = latestTimestamp.VpgName;
                consolidatedObject.VmIdInternalVmName = latestTimestamp.VmIdInternalVmName;
                consolidatedObject.MemoryReservedInMB = latestTimestamp.MemoryReservedInMB;
                consolidatedObject.MemoryInMB = latestTimestamp.MemoryInMB;
                consolidatedObject.MemoryLimitInMB= latestTimestamp.MemoryLimitInMB;

                // remove all elements of this VM from the original list as we are about to add the consolidated object
                m_VMResourceInfoStorageObjects.RemoveAll(x => x.VmIdInternalVmName == VM.VmIdentifierInternalVmName);
                m_VMResourceInfoStorageObjects.Add(consolidatedObject);
            }
        }
    }
}
