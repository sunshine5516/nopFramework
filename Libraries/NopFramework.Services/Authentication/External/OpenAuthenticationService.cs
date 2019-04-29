using System;
using System.Collections.Generic;
using System.Linq;
using NopFramework.Core.Data;
using NopFramework.Core.Domains.Customers;
using NopFramework.Core.Plugins;
using NopFramework.Services.Customers;

namespace NopFramework.Services.Authentication.External
{
    /// <summary>
    /// 开放授权服务
    /// </summary>
    public partial class OpenAuthenticationService : IOpenAuthenticationService
    {
        #region 声明实例
        private readonly ICustomerService _customerService;
        private readonly IPluginFinder _pluginFinder;
        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
        private readonly IRepository<ExternalAuthenticationRecord> _externalAuthenticationRecordRepository;
        #endregion
        #region 构造函数
        public OpenAuthenticationService(ICustomerService customerService, IPluginFinder pluginFinder,
            IRepository<ExternalAuthenticationRecord> externalAuthenticationRecordRepository)
        {
            this._customerService = customerService;
            this._pluginFinder = pluginFinder;
            this._externalAuthenticationRecordRepository = externalAuthenticationRecordRepository;
        }
        #endregion
        #region 方法
        public bool AccountExists(OpenAuthenticationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public virtual void AssociateExternalAccountWithUser(Customer customer, OpenAuthenticationParameters parameters)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");
            string email = null;
            if (parameters.UserClaims != null)
            {
                foreach (var userClaim in parameters.UserClaims.Where(x => x.Contact != null &&
                 !String.IsNullOrEmpty(x.Contact.Email)))
                {
                    email = userClaim.Contact.Email;
                    break;
                }
            }
            var externalAuthenticationRecord = new ExternalAuthenticationRecord
            {
                CustomerId = customer.Id,
                Email = email,
                ExternalDisplayIdentifier = parameters.ExternalDisplayIdentifier,
                ExternalIdentifier = parameters.ExternalIdentifier,
                OAuthToken = parameters.OAuthToken,
                OAuthAccessToken = parameters.OAuthAccessToken,
                ProviderSystemName = parameters.ProviderSystemName,
            };
            _externalAuthenticationRecordRepository.Insert(externalAuthenticationRecord);
        }

        public void DeletExternalAuthenticationRecord(ExternalAuthenticationRecord externalAuthenticationRecord)
        {

            throw new NotImplementedException();
        }

        public IList<ExternalAuthenticationRecord> GetExternalIdentifiersFor(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Customer GetUser(OpenAuthenticationParameters parameters)
        {
            var record = _externalAuthenticationRecordRepository.Table
                .FirstOrDefault(o => o.ExternalDisplayIdentifier == parameters.ExternalDisplayIdentifier &&
                o.ProviderSystemName == parameters.ProviderSystemName);
            if (record != null)
                return _customerService.GetCustomerById(record.CustomerId);
            return null;
        }

        public IList<IExternalAuthenticationMethod> LoadActiveExternalAuthenticationMethods(int storeId = 0)
        {
            return LoadAllExternalAuthenticationMethods(storeId);
        }

        public IList<IExternalAuthenticationMethod> LoadAllExternalAuthenticationMethods(int storeId = 0)
        {
            return _pluginFinder.GetPlugins<IExternalAuthenticationMethod>().ToList();
        }
        /// <summary>
        /// 根据名称加载所有的授权方法
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        public IExternalAuthenticationMethod LoadExternalAuthenticationMethodBySystemName(string systemName)
        {
            var descriptor = _pluginFinder.GetPluginDescriptorBySystemName<IExternalAuthenticationMethod>(systemName);
            if (descriptor != null)
                return descriptor.Instance<IExternalAuthenticationMethod>();
            return null;
            //throw new NotImplementedException();
        }

        public void RemoveAssociation(OpenAuthenticationParameters parameters)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
