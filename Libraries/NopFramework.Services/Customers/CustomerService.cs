using System;
using System.Collections.Generic;
using System.Linq;
using NopFramework.Core;
using NopFramework.Core.Domains.Customers;
using NopFramework.Core.Data;
using NopFramework.Services.Events;
using NopFramework.Core.Caching;
namespace NopFramework.Services.Customers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CustomerService : ICustomerService
    {
        #region 常量
        /// <summary>
        /// 
        /// </summary>
        private const string CUSTOMERROLES_BY_SYSTEMNAME_KEY = "NopFramework.customerrole.systemname-{0}";
        /// <summary>
        ///所有用户角色
        /// </summary>
        private const string CUSTOMERROLES_ALL_KEY = "NopFramework.customerrole.all-{0}";
        #endregion
        #region 声明实例
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerRole> _customerRoleRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        #endregion
        #region 构造函数
        public CustomerService(IRepository<Customer> customreRepository,
            IRepository<CustomerRole> customerRoleRepository, ICacheManager cacheManager,
            IEventPublisher eventPublisher)
        {
            this._customerRepository = customreRepository;
            this._customerRoleRepository = customerRoleRepository;
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
        }
        #endregion
        #region 用户管理
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="customer"></param>
        public void DeleteCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");
            if (customer.IsSystemAccount)
                throw new SunFrameworkException(string.Format("系统用户账号{0}不能被删除", customer.SystemName));
            customer.Deleted = true;
            UpdateCustomer(customer);
        }
        /// <summary>
        /// 分页用户
        /// </summary>
        /// <param name="createdFrom"></param>
        /// <param name="createdTo"></param>
        /// <param name="customerRoleIds"></param>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="dayOfBirth"></param>
        /// <param name="monthOfBirth"></param>
        /// <param name="phone"></param>
        /// <param name="ipAddress"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<Customer> GetAllCustomers(DateTime? createdFrom = default(DateTime?),
            DateTime? createdTo = default(DateTime?), int[] customerRoleIds = null,
            string email = null, string userName = null, string firstName = null,
            string lastName = null, int dayOfBirth = 0, int monthOfBirth = 0, string phone = null,
            string ipAddress = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _customerRepository.Table;
            if (createdFrom.HasValue)
                query = query.Where(c => createdFrom <= c.CreatedOn);
            if (createdTo.HasValue)
                query = query.Where(c => createdTo >= c.CreatedOn);
            query = query.Where(c => !c.Deleted);

            if (customerRoleIds != null && customerRoleIds.Length > 0)
                if (customerRoleIds[0] != 0)
                    query = query.Where(c => c.CustomerRoles.Select(cr => cr.Id).Intersect(customerRoleIds).Any());
            if (!String.IsNullOrWhiteSpace(email))
                query = query.Where(c => c.Email.Contains(email));
            if (!String.IsNullOrWhiteSpace(userName))
                query = query.Where(c => c.UserName.Contains(userName));
            query = query.OrderByDescending(c => c.CreatedOn);
            var customers = new PagedList<Customer>(query, pageIndex, pageSize);
            return customers;

        }

        public IList<Customer> GetAllCustomersByPasswordFormat(PasswordFormat passwordFormat)
        {
            var passwordFormatId = (int)passwordFormat;

            var query = _customerRepository.Table;
            query = query.Where(c => c.PasswordFormatId == passwordFormatId);
            query = query.OrderByDescending(c => c.CreatedOn);
            var customers = query.ToList();
            return customers;
        }
        /// <summary>
        /// 根据电子邮件获取用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Customer GetCustomerByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;
            var query = _customerRepository.Table.Where(c => c.Email == email).OrderBy(c => c.Id);
            var customer = query.FirstOrDefault();
            return customer;
        }
        /// <summary>
        /// 通过GUID获取客户
        /// </summary>
        /// <param name="customerGuid"></param>
        /// <returns></returns>
        public Customer GetCustomerByGuid(Guid customerGuid)
        {
            if (customerGuid == Guid.Empty)
                return null;
            var query = _customerRepository.Table.Where(c => c.CustomerGuid == customerGuid).OrderBy(c => c.Id);
            var customer = query.FirstOrDefault();
            return customer;
        }
        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public Customer GetCustomerById(int customerId)
        {
            if (customerId == 0)
                return null;
            return _customerRepository.GetById(customerId);
        }
        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Customer GetCustomerByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;
            var query = _customerRepository.Table.Where(c => c.UserName == username).FirstOrDefault();
            return query;
        }
        /// <summary>
        /// 根据用户ID获取用户
        /// </summary>
        /// <param name="customerIds"></param>
        /// <returns></returns>
        public IList<Customer> GetCustomersbyIds(int[] customerIds)
        {
            if (customerIds == null || customerIds.Length == 0)
                return new List<Customer>();
            List<Customer> customers = new List<Customer>();
            foreach (var id in customerIds)
            {
                var customer = _customerRepository.GetById(id);
                if (customer != null)
                {
                    customers.Add(customer);
                }
            }
            return customers;
        }

        public IPagedList<Customer> GetOnlineCustomers(DateTime lastActivityFrom, 
            int[] customerRoleIds, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _customerRepository.Table;
            query = query.Where(c => c.LastActivityDate >= lastActivityFrom);
            query = query.Where(c => !c.Deleted);
            if (customerRoleIds != null && customerRoleIds.Length > 0)
                query = query.Where(c => c.CustomerRoles.Select(cr => cr.Id).Intersect(customerRoleIds).Any());
            query = query.OrderByDescending(c => c.LastActivityDate);
            var customers = new PagedList<Customer>(query, pageIndex, pageSize);
            return customers;
        }

        public void InsertCustomer(Customer customer)
        {
            if (customer != null)
                _customerRepository.Insert(customer);
            //event notification
            // _eventPublisher.EntityInserted(customer);
        }
        /// <summary>
        /// 添加一个游客用户
        /// </summary>
        /// <returns></returns>
        public Customer InsertGuestCustomer()
        {
            var customer = new Customer
            {
                CustomerGuid = Guid.NewGuid(),
                IsActive = true,
                CreatedOn = DateTime.Now,
                LastActivityDate = DateTime.Now
            };
            var guestRole = GetCustomerRoleBySystemName(SystemCustomerRoleNames.Guests);
            if (guestRole == null)
                throw new ArgumentNullException("'Guests' role could not be loaded");
            customer.CustomerRoles.Add(guestRole);
            _customerRepository.Insert(customer);

            return customer;
        }

        public void UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");
            _customerRepository.Update(customer);
            //_eventPublisher.EntityUpdated(customer);

        }
        public Customer GetCustomerBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;
            return _customerRepository.Table.Where(p => p.SystemName == systemName).FirstOrDefault();
        }
        #endregion
        #region 用户角色管理
        public void DeleteCustomerRole(CustomerRole customerRole)
        {
            if (customerRole == null)
                throw new ArgumentNullException("customerRole");

            if (customerRole.IsSystemRole)
                throw new SunFrameworkException("System role could not be deleted");
            _customerRoleRepository.Delete(customerRole);
            _cacheManager.RemoveByPattern(CUSTOMERROLES_ALL_KEY);
        }
        /// <summary>
        /// 获取所有用户角色
        /// </summary>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        public IList<CustomerRole> GetAllCustomerRoles(bool showHidden = false)
        {
            //string key = string.Format(CUSTOMERROLES_ALL_KEY, showHidden);
            //return _cacheManager.Get(key,()=>{
            var customerRoles = _customerRoleRepository.Table.Where(cr => cr.IsActive || showHidden).ToList();
            return customerRoles;
            //});
        }
        public IPagedList<CustomerRole> GetAllPagedList(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //string key = string.Format(CUSTOMERROLES_ALL_KEY,true);
            //return _cacheManager.Get(key, () => {
            var query = _customerRoleRepository.Table.Where(cr => cr.IsActive).ToList();
            var customerRoles = new PagedList<CustomerRole>(query, pageIndex, pageSize);
            return customerRoles;
            //});
        }


        public CustomerRole GetCustomerRoleById(int customerRoleId)
        {
            return _customerRoleRepository.GetById(customerRoleId);

        }
        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        public CustomerRole GetCustomerRoleBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;
            //string key = string.Format(CUSTOMERROLES_BY_SYSTEMNAME_KEY, systemName);
            //return _cacheManager.Get(key, () =>
            //{
            var customerRoles = _customerRoleRepository.Table.Where(
                cr => cr.SystemName == systemName).FirstOrDefault();
            return customerRoles;
            //});
        }
        public void InsertCustomerRole(CustomerRole customerRole)
        {
            if (customerRole != null)
            {
                _customerRoleRepository.Insert(customerRole);
            }
            _eventPublisher.EntityInserted<CustomerRole>(customerRole);
        }


        public void UpdateCustomerRole(CustomerRole customerRole)
        {
            if (customerRole == null)
                throw new ArgumentNullException("customerRole");

            _customerRoleRepository.Update(customerRole);
        }

        public IList<CustomerRole> GetCustomerRoleByIds(int[] customerRoleIds)
        {
            if (customerRoleIds == null || customerRoleIds.Length == 0)
                return new List<CustomerRole>();
            //var query = _customerRoleRepository.Table.Where((p=>p.Id.c));
            var query = from cr in _customerRoleRepository.Table
                        where customerRoleIds.Contains(cr.Id)
                        select cr;
            var roles = query.ToList();
            var rolesList = new List<CustomerRole>();
            foreach (var role in roles)
            {
                //var role = _customerRoleRepository.GetById(id);
                rolesList.Add(role);
            }
            return rolesList;
        }

        public void DeleteCustomerRoles(IList<CustomerRole> customerRoles)
        {
            if (customerRoles == null)
            {
                throw new ArgumentNullException("customerRoles");
            }
            _customerRoleRepository.Delete(customerRoles);
        }


        #endregion
        #region 方法

        #endregion
    }
}
