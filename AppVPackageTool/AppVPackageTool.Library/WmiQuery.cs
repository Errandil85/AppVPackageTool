using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Windows.Forms;

namespace AppVPackageTool.Library
{
    public class WmiQuery
    {
        public static List<ListAppvPackages> GetAppvPackagesLocalHost()
        {
            List<ListAppvPackages> ListAppvPackages = new List<ListAppvPackages>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("ROOT\\APPV", "select Name, Version, PackageId, VersionID, IsPublishedGlobally, InUse from AppvClientPackage");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                ListAppvPackages.Add(new ListAppvPackages()
                {
                    Name = queryObj["Name"].ToString(),
                    Version = queryObj["Version"].ToString(),
                    PackageID = queryObj["PackageID"].ToString(),
                    VersionID = queryObj["VersionID"].ToString(),
                    IsPublishedGlobally = Convert.ToBoolean(queryObj["IsPublishedGlobally"]),
                    InUse = Convert.ToBoolean(queryObj["InUse"]),
                });
            }
            return ListAppvPackages;
        }

        public static List<ListAppvPackages> GetAppvPackagesRemote(string hostname)
        {
            List<ListAppvPackages> ListAppvPackages = new List<ListAppvPackages>();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher($"\\\\{hostname}\\ROOT\\APPV", "select Name, Version, PackageId, VersionID, IsPublishedGlobally, InUse from AppvClientPackage");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    ListAppvPackages.Add(new ListAppvPackages()
                    {
                        Name = queryObj["Name"].ToString(),
                        Version = queryObj["Version"].ToString(),
                        PackageID = queryObj["PackageID"].ToString(),
                        VersionID = queryObj["VersionID"].ToString(),
                        IsPublishedGlobally = Convert.ToBoolean(queryObj["IsPublishedGlobally"]),
                        InUse = Convert.ToBoolean(queryObj["InUse"]),
                    });
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return ListAppvPackages;
        }

        public static void RemoveAppvPackageLocalHost(string packageID, string versionID)
        {
            object[] cmd = { $"PowerShell.exe Get-AppvClientPackage -PackageId {packageID} -VersionId {versionID} | Unpublish-AppvClientPackage -Global | Remove-AppvClientPackage", null, null, 0 };

            ManagementClass removeAppvPackage = new ManagementClass(@"\root\cimv2:Win32_Process");

            try
            {
                removeAppvPackage.InvokeMethod("Create", cmd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void RemoveAppvPackageLocalHostUser(string packageID, string versionID)
        {
            object[] cmd = { $"PowerShell.exe Get-AppvClientPackage -PackageId {packageID} -VersionId {versionID} | Unpublish-AppvClientPackage | Remove-AppvClientPackage", null, null, 0 };

            ManagementClass removeAppvPackage = new ManagementClass(@"\root\cimv2:Win32_Process");
            try
            {
                removeAppvPackage.InvokeMethod("Create", cmd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void RemoveAppvPackageRemote(string packageID, string versionID, string hostname)
        {
            object[] cmd = { $"PowerShell.exe Get-AppvClientPackage -PackageId {packageID} -VersionId {versionID} | Unpublish-AppvClientPackage -Global | Remove-AppvClientPackage", null, null, 0 };

            ManagementClass removeAppvPackage = new ManagementClass($"\\\\{hostname}\\root\\cimv2:Win32_Process");

            try
            {
                removeAppvPackage.InvokeMethod("Create", cmd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void RemoveAppvPackageRemoteUser(string packageID, string versionID, string hostname)
        {
            object[] cmd = { $"PowerShell.exe Get-AppvClientPackage -PackageId {packageID} -VersionId {versionID} | Unpublish-AppvClientPackage | Remove-AppvClientPackage", null, null, 0 };

            ManagementClass removeAppvPackage = new ManagementClass($"\\\\{hostname}\\root\\cimv2:Win32_Process");

            try
            {
                removeAppvPackage.InvokeMethod("Create", cmd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void StopAppvClientPackageLocalhost(string packageID, string versionID)
        {
            object[] cmd = { $"PowerShell.exe Stop-AppvClientPackage -PackageId {packageID} -VersionId {versionID}", null, null, 0 };

            ManagementClass removeAppvPackage = new ManagementClass(@"\root\cimv2:Win32_Process");

            try
            {
                removeAppvPackage.InvokeMethod("Create", cmd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void StopAppvClientPackageRemote(string packageID, string versionID, string hostname)
        {
            object[] cmd = { $"PowerShell.exe Stop-AppvClientPackage -PackageId {packageID} -VersionId {versionID}", null, null, 0 };

            ManagementClass removeAppvPackage = new ManagementClass($"\\\\{hostname}\\root\\cimv2:Win32_Process");

            try
            {
                removeAppvPackage.InvokeMethod("Create", cmd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
