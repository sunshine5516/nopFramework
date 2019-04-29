using NopFramework.Core;
using NopFramework.Core.Domains.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Common
{
    /// <summary>
    /// 常规属性服务接口
    /// </summary>
    public partial interface IGenericAttributeService
    {
        void DeleteAttribute(GenericAttribute attribute);
        void DeleteAttributes(IList<GenericAttribute> attributes);
        GenericAttribute GetAttribute(int attributeId);
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="attribute">attribute</param>
        void InsertAttribute(GenericAttribute attribute);

        /// <summary>
        /// 更新属性
        /// </summary>
        /// <param name="attribute">Attribute</param>
        void UpdateAttribute(GenericAttribute attribute);
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="keyGroup">Key group</param>
        /// <returns>Get attributes</returns>
        IList<GenericAttribute> GetAttributesForEntity(int entityId, string keyGroup);

        /// <summary>
        /// 保存属性值
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        void SaveAttribute<TPropType>(BaseEntity entity, string key, TPropType value);
    }
}
