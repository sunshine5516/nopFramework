using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Customers
{
    /// <summary>
    /// 注册结果
    /// </summary>
    public class CustomerRegistrationResult
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public IList<string> Errors { get; set; }
        public CustomerRegistrationResult()
        {
            this.Errors = new List<string>();
        }
        public bool Success
        {
            get { return !this.Errors.Any(); }
        }
        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="error"></param>
        public void AddErrors(string error)
        {
            this.Errors.Add(error);
        }
    }
}
