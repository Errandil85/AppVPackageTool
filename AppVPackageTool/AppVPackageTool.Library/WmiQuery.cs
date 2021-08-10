using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace AppVPackageTool.Library
{
    public class WmiQuery
    {
        public static List<ListAppvPackages> GetAppvPackagesLocalHost()
        {
            List<ListAppvPackages> ListAppvPackages = new List<ListAppvPackages>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("ROOT\\APPV", "select Name, Version, PackageId, VersionID from AppvClientPackage");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                //Console.WriteLine("{0}", queryObj["Name"]);
                ListAppvPackages.Add(new ListAppvPackages()
                {
                    Name = queryObj["Name"].ToString(),
                    Version = queryObj["Version"].ToString(),
                    PackageID = queryObj["PackageID"].ToString(),
                    VersionID = queryObj["VersionID"].ToString(),
                });
            }
            return ListAppvPackages;
        }

        public static List<ListAppvPackages> GetAppvPackagesRemote(string hostname)
        {
            List<ListAppvPackages> ListAppvPackages = new List<ListAppvPackages>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher($"\\\\{hostname}\\ROOT\\APPV", "select Name, Version, PackageId, VersionID from AppvClientPackage");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                //Console.WriteLine("{0}", queryObj["Name"]);
                ListAppvPackages.Add(new ListAppvPackages()
                {
                    Name = queryObj["Name"].ToString(),
                    Version = queryObj["Version"].ToString(),
                    PackageID = queryObj["PackageID"].ToString(),
                    VersionID = queryObj["VersionID"].ToString(),
                });
            }
            return ListAppvPackages;
        }

        public static void RemoveAppvPackageLocalHost(string packageID, string versionID)
        {
            //object[] cmd = { $"PowerShell.exe Remove-AppvClientPackage -PackageId {packageID} -VersionId {versionID}", null, null, 0 };
            object[] cmd = { $"PowerShell.exe Get-AppvClientPackage -PackageId {packageID} -VersionId {versionID} | Unpublish-AppvClientPackage -Global | Remove-AppvClientPackage", null, null, 0 };

            ManagementClass removeAppvPackage = new ManagementClass(@"\root\cimv2:Win32_Process");
            removeAppvPackage.InvokeMethod("Create", cmd);
        }

        public static void RemoveAppvPackageRemote(string packageID, string versionID, string hostname)
        {
            //object[] cmd = { $"PowerShell.exe Remove-AppvClientPackage -PackageId {packageID} -VersionId {versionID}", null, null, 0 };
            object[] cmd = { $"PowerShell.exe Get-AppvClientPackage -PackageId {packageID} -VersionId {versionID} | Unpublish-AppvClientPackage -Global | Remove-AppvClientPackage", null, null, 0 };

            ManagementClass removeAppvPackage = new ManagementClass($"\\\\{hostname}\\root\\cimv2:Win32_Process");
            removeAppvPackage.InvokeMethod("Create", cmd);
        }
    }
}
