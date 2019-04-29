using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;

namespace NopFramework.Admin.Models.Common
{
    public partial class SystemInfoModel: BaseNopFrameworkModel
    {
        public SystemInfoModel()
        {
            this.ServerVariables = new List<ServerVariableModel>();
            this.LoadedAssemblies = new List<LoadedAssembly>();
        }
        [NopFrameworkResourceDisplayName("版本信息")]
        public string SysVersion { get; set; }
        [NopFrameworkResourceDisplayName("操作系统")]
        public string OperationSystem { get; set; }
        [NopFrameworkResourceDisplayName("ASP.NET版本")]
        public string AspNetInfo { get; set; }
        [NopFrameworkResourceDisplayName("完全信任级别	")]
        public string IsFullTrust { get; set; }
        [NopFrameworkResourceDisplayName("服务器时区")]
        public DateTime ServerLocalTime { get; set; }
        [NopFrameworkResourceDisplayName("Admin.System.SystemInfo.ServerVariables")]
        public DateTime UtcTime { get; set; }
        [NopFrameworkResourceDisplayName("Admin.System.SystemInfo.ServerVariables")]
        public DateTime CurrentUserTime { get; set; }
        [NopFrameworkResourceDisplayName("Admin.System.SystemInfo.ServerVariables")]
        public string ServerTimeZone { get; set; }
        [NopFrameworkResourceDisplayName("Admin.System.SystemInfo.ServerVariables")]
        public string HttpHost { get; set; }

        [NopFrameworkResourceDisplayName("Admin.System.SystemInfo.ServerVariables")]
        public IList<ServerVariableModel> ServerVariables { get; set; }

        [NopFrameworkResourceDisplayName("Admin.System.SystemInfo.LoadedAssemblies")]
        public IList<LoadedAssembly> LoadedAssemblies { get; set; }
        public partial class ServerVariableModel : BaseNopFrameworkModel
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        public partial class LoadedAssembly : BaseNopFrameworkModel
        {
            public string FullName { get; set; }
            public string Location { get; set; }
        }
    }
}