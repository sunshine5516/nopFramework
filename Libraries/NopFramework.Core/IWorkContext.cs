using NopFramework.Core.Domains.Customers;
using NopFramework.Core.Domains.Localization;
namespace NopFramework.Core
{
    public interface IWorkContext
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        Customer CurrentCustomer { get; set; }
        /// <summary>
        ///获取或设置当前语言
        /// </summary>
        Language WorkingLanguage { get; set; }
        /// <summary>
        /// 原始客户（如果当前被盗用）
        /// </summary>
        Customer OriginalCustomerIfImpersonated { get; }
        bool IsAdmin { get; set; }
    }
}
