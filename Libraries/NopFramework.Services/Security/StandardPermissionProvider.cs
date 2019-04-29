using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core.Domains.Security;

namespace NopFramework.Services.Security
{
    public partial class StandardPermissionProvider : IPermissionProvider
    {
        public static readonly PermissionRecord AccessAdminPanel = new PermissionRecord { Name = "Access admin area", SystemName = "AccessAdminPanel", Category = "Standard" };//访问后台
        public static readonly PermissionRecord ManageActivityLog = new PermissionRecord { Name = "Admin area. Manage Activity Log", SystemName = "ManageActivityLog", Category = "Configuration" };//日志管理
        public static readonly PermissionRecord ManageCustomers = new PermissionRecord { Name = "Admin area.Manage Customers", SystemName = "ManageCustomers", Category = "Customers" };//用户管理
        public static readonly PermissionRecord ManageNews = new PermissionRecord { Name = "Admin area. Manage News", SystemName = "ManageNews", Category = "Content Management" };//新闻管理
        //public static readonly PermissionRecord ManageMaintenance=new PermissionRecord { Name = "Admin area. Manage Maintenance", SystemName = "ManageMaintenance", Category = "Configuration" };//友好路由管理
        public static readonly PermissionRecord ManageProjects = new PermissionRecord { Name = "Admin area. Manage Projects", SystemName = "ManageProjects", Category = "Organization" };//项目管理
        public static readonly PermissionRecord ManageBugs = new PermissionRecord { Name = "Admin area. Manage Bugs", SystemName = "ManageBugs", Category = "Organization" };//Bug管理
        public static readonly PermissionRecord ManageAllBugs = new PermissionRecord { Name = "Admin area. Manage Bugs", SystemName = "ManageBugs", Category = "Organization" };//Bug管理

        public static readonly PermissionRecord ManageProducts = new PermissionRecord { Name = "Admin area. Manage Products", SystemName = "ManageProducts", Category = "Catalog" };//商品管理
        public static readonly PermissionRecord ManageTerms = new PermissionRecord { Name = "Admin area. Manage Terms", SystemName = "ManageTerms", Category = "Content Management" };//词条管理

        public static readonly PermissionRecord ManageVendors = new PermissionRecord { Name = "Admin area. Manage Vendors", SystemName = "ManageVendors", Category = "Customers" };//供应商管理
        public static readonly PermissionRecord ManageRecruitment = new PermissionRecord { Name = "Admin area. Manage Recruitment", SystemName = "ManageRecruitment", Category = "Organization" };//职位管理

        public static readonly PermissionRecord ManageResource = new PermissionRecord { Name = "Admin area. Manage Resource", SystemName = "ManageResource", Category = "Configuration" };//资源文件管理



        public static readonly PermissionRecord ManageMachine = new PermissionRecord { Name = "Admin area. Manage Machine", SystemName = "ManageMachine", Category = "Inventory" };//机械管理
        public static readonly PermissionRecord ManageElectron = new PermissionRecord { Name = "Admin area. Manage Electron", SystemName = "ManageElectron", Category = "Inventory" };//电子管理
        public static readonly PermissionRecord ManageExternalAuthenticationMethods = new PermissionRecord { Name = "Admin area. Manage External Authentication Methods", SystemName = "ManageExternalAuthenticationMethods", Category = "Configuration" };//第三方授权
        public static readonly PermissionRecord ManageMaintenance = new PermissionRecord { Name = "Admin area. Manage Maintenance", SystemName = "ManageMaintenance", Category = "Configuration" };//系统信息
        public static readonly PermissionRecord ManageLanguages = new PermissionRecord { Name = "Admin area. Manage Languages", SystemName = "ManageLanguages", Category = "Configuration" };//语言管理

        public IEnumerable<DefaultPermissionRecord> GetDefaultPermissions()
        {
            return new[]
            {
                new DefaultPermissionRecord{
                    PermissionRecords=new[]
                    {
                         AccessAdminPanel
                    }
                }
            };
        }

        public IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[] {
                AccessAdminPanel
            };
        }
    }
}
