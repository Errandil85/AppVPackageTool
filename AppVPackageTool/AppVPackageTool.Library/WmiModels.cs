using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppVPackageTool.Library
{
    public class WmiModels
    {
    }

    /// <summary>
    /// List model for appv packages
    /// </summary>
    public class ListAppvPackages
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string PackageID { get; set; }
        public string VersionID { get; set; }
        public bool IsPublishedGlobally { get; set; }
        public bool InUse { get; set; }
    }
}
