using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core;
using NopFramework.Core.Domains.Messages;
using NopFramework.Core.Data;

namespace NopFramework.Services.Messages
{
    public partial class ContactMessageService : IContactMessageService
    {
        #region 生命实例
        private readonly IRepository<ContactMessage> _contactMessageRepository;
        #endregion
        #region 构造函数
        public ContactMessageService(IRepository<ContactMessage> contactMessageRepository)
        {
            this._contactMessageRepository = contactMessageRepository;
        }
        #endregion
        #region 方法
        public void DeleteContactMessage(ContactMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("ContactMessage");
            }
            _contactMessageRepository.Delete(message);
        }

        public IList<ContactMessage> GetAllContactMessage()
        {
            var query = _contactMessageRepository.Table.OrderByDescending
                (cm => cm.CreatedOn).ToList();
            return query;
        }
        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public ContactMessage GetMessageById(int messageId)
        {
            if (messageId == 0)
                return null;
            return _contactMessageRepository.GetById(messageId);
        }

        public IPagedList<ContactMessage> GetMessageByPaged(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _contactMessageRepository.Table.OrderByDescending
               (cm => cm.CreatedOn).ToList();
            var message = new PagedList<ContactMessage>(query, pageIndex, pageSize);
            return message;

        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="message"></param>
        public void InsertContactMessage(ContactMessage message)
        {
            if (message == null)
                throw new ArgumentNullException("ContactMessage");
            _contactMessageRepository.Insert(message);
        }

        public void UpdateContactMessage(ContactMessage message)
        {
            if (message == null)
                throw new ArgumentNullException("ContactMessage");
            _contactMessageRepository.Update(message);
        }
        #endregion

    }
}
