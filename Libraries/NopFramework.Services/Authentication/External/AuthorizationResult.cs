using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Authentication.External
{
    /// <summary>
    /// 授权结果
    /// </summary>
    public partial class AuthorizationResult
    {
        #region 声明实例
        public IList<string> Errors { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public OpenAuthenticationStatus Status { get; private set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success
        {
            get { return !this.Errors.Any(); }
        }
        /// <summary>
        /// 添加错误
        /// </summary>
        /// <param name="error">Error</param>
        public void AddError(string error)
        {
            this.Errors.Add(error);
        }
        #endregion
        #region 构造函数
        public AuthorizationResult(OpenAuthenticationStatus status)
        {
            this.Status = status;
            this.Errors = new List<string>();
        }
        #endregion
    }
}
