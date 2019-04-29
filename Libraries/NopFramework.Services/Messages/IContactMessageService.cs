using NopFramework.Core;
using NopFramework.Core.Domains.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Messages
{
    /// <summary>
    /// 联系消息接口
    /// </summary>
    public partial interface IContactMessageService
    {
        #region 接受消息管理
        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="message">Activity log type item</param>
        void InsertContactMessage(ContactMessage message);

        /// <summary>
        /// 更新消息
        /// </summary>
        /// <param name="message">Activity log type item</param>
        void UpdateContactMessage(ContactMessage message);

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="activityLogType"></param>
        void DeleteContactMessage(ContactMessage message);

        /// <summary>
        /// 获取所有联系信息
        /// </summary>
        /// <returns>Activity log type items</returns>
        IList<ContactMessage> GetAllContactMessage();
        IPagedList<ContactMessage> GetMessageByPaged(int pageIndex = 0, int pageSize = int.MaxValue);
        /// <summary>
        /// 根据ID获取消息
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>Activity log type item</returns>
        ContactMessage GetMessageById(int messageId);
        #endregion
    }
}
