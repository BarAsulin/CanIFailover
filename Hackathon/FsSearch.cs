using System.Data;
using System.Linq;
using System.IO;
using System;

namespace CanIFailover
{
    public class FilesystemSearch
    {
        public static string GetStorageProperties()
        {

            foreach (DriveInfo drive in DriveInfo.GetDrives().Where(drive => drive.IsReady == true && drive.DriveType == DriveType.Fixed))
            {
                if (File.Exists(drive.RootDirectory.FullName + @"Program Files\Zerto\Zerto Virtual Replication\storage_properties.xml"))
                {
                    return drive.RootDirectory.FullName + @"Program Files\Zerto\Zerto Virtual Replication\storage_properties.xml";
                }
            };
            throw new Exception("Failed to find storage_properties.xml");
        }
    }
}