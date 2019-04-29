
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Customers
{
    /// <summary>
    /// 用户
    /// </summary>
    public partial class Customer:BaseEntity
    {
        private ICollection<CustomerRole> _customerRoles;
        public Customer()
        {
            this.CustomerGuid = Guid.NewGuid();
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid CustomerGuid { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        public string Email { get; set; }//电子邮件
        public string Password { get; set; }//密码
        public int PasswordFormatId { get; set; }//密码格式
        public PasswordFormat PasswordFormat {
            get { return (PasswordFormat)PasswordFormatId; }
            set { this.PasswordFormatId = (int)value; }
        }
        public string PasswordSalt { get; set; }//加盐值
        public string AdminComment { get; set; }//管理员评论
        public string AffiliatedId { get; set; }//会员标识符
        public bool IsActive { get; set; }//是否活跃
        public bool Deleted { get; set; }//是否被删除
        public bool IsSystemAccount { get; set; }//客户帐户是否为系统
        public string SystemName { get; set; }//获取或设置客户系统名称
        public string LastIpAddress { get; set; }//最新IpAddress
        //public DateTime CreatedOn { get; set; }//添加时间
        public DateTime? LastLoginDate { get; set; }//最后登录时间
        public DateTime LastActivityDate { get; set; }//上次活动的日期和时间
        /// <summary>
        /// 客户角色
        /// </summary>
        public virtual ICollection<CustomerRole> CustomerRoles
        {
            get { return _customerRoles ?? (_customerRoles = new List<CustomerRole>()); }
            protected set { _customerRoles = value; }
        }
        /// <summary>
        /// 部门
        /// </summary>

    }
}
