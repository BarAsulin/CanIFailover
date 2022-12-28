using System;
using System.Collections.Generic;
using CanIFailover.VCObjects;
using VMware.Vim;

namespace CanIFailover
{
    class vCenterConnector
    {
        public static VimClientImpl ConnectTovCenter(string vcenterIP, string username, string password)
        {
            VimClientImpl client = new VimClientImpl();
            client.IgnoreServerCertificateErrors = true;
            TryToConnect(client, vcenterIP);
            TryToLogin(client, username, password);
            return client;
        }
        public static string GetVCenterGuidAsString(VimClientImpl client)
        {
            return client.ServiceContent.About.InstanceUuid;
        }
        public static List<EsxHost> GetEsxHosts(VimClientImpl client)
        {
            try
            {
                var hosts = client.FindEntityViews(typeof(HostSystem), null, null, null);
                List<EsxHost> returnList = new List<EsxHost>(hosts.Count);
                foreach (var host in hosts)
                {
                    var hostSystem = (HostSystem)host;
                    var memInfo = hostSystem.Summary.Hardware.MemorySize;
                    var memAvail = hostSystem.Summary.QuickStats.OverallMemoryUsage;
                    var cpuMhz = hostSystem.Summary.Hardware.CpuMhz;
                    var cpuTotal = hostSystem.Summary.Hardware.NumCpuCores * cpuMhz;
                    var cpuUsed = hostSystem.Summary.QuickStats.OverallCpuUsage;
                    var coresUsed = (int)Math.Round((double)cpuUsed / (double)cpuMhz);
                    var coresAvail = hostSystem.Summary.Hardware.NumCpuCores - coresUsed;
                    returnList.Add(new EsxHost(hostSystem.MoRef.Value, hostSystem.Name, memInfo, (long)memAvail, cpuTotal, (int)cpuUsed, coresAvail, coresUsed));
                }
                return returnList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public static bool TryToLogin(VimClientImpl client,string username, string password)
        {
            try
            {
                client.Login("root", "Zertodata1!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            return true;
        }
        public static bool TryToConnect(VimClientImpl client, string vcenterIP)
        {
            try
            {
                client.Connect(GetConnectionUrl(vcenterIP));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            return true;
        }
        public static string GetConnectionUrl(string vcenterIP)
        {
            return string.Format("https://{0}/sdk", vcenterIP);
        }
    }
}
