﻿using NopFramework.Core.Domains.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Authentication.External
{
    /// <summary>
    /// 开放服务授权接口
    /// </summary>
    public partial interface IOpenAuthenticationService
    {
        /// <summary>
        /// 加载所有启用的授权方法
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        IList<IExternalAuthenticationMethod> LoadActiveExternalAuthenticationMethods(int storeId = 0);
        /// <summary>
        /// 根据名名称加载所有的授权方法
        /// </summary>
        /// <param name="systemName">系统名</param>
        /// <returns>Found external authentication method</returns>
        IExternalAuthenticationMethod LoadExternalAuthenticationMethodBySystemName(string systemName);

        /// <summary>
        /// 加载所有的授权方法
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns>External authentication methods</returns>
        IList<IExternalAuthenticationMethod> LoadAllExternalAuthenticationMethods(int storeId = 0);
        bool AccountExists(OpenAuthenticationParameters parameters);
        void AssociateExternalAccountWithUser(Customer customer, OpenAuthenticationParameters parameters);
        Customer GetUser(OpenAuthenticationParameters parameters);
        IList<ExternalAuthenticationRecord> GetExternalIdentifiersFor(Customer customer);
        void DeletExternalAuthenticationRecord(ExternalAuthenticationRecord externalAuthenticationRecord);
        void RemoveAssociation(OpenAuthenticationParameters parameters);
    }
}
