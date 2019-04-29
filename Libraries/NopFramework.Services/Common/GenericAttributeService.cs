using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core;
using NopFramework.Core.Domains.Common;
using NopFramework.Core.Data;
using NopFramework.Core.Caching;
using NopFramework.Data;

namespace NopFramework.Services.Common
{
    public partial class GenericAttributeService : IGenericAttributeService
    {
        #region 常量
       
        #endregion
        #region 声明实例
        private readonly IRepository<GenericAttribute> _genericAttributeRepository;
        private readonly ICacheManager _cacheManage;
        #endregion
        #region 构造函数
        public GenericAttributeService(IRepository<GenericAttribute> genericAttributeRepository
            ,ICacheManager cacheManager)
        {
            this._genericAttributeRepository = genericAttributeRepository;
            this._cacheManage = cacheManager;
        }
        #endregion

        #region 方法
        public void DeleteAttribute(GenericAttribute attribute)
        {
            throw new NotImplementedException();
        }

        public void DeleteAttributes(IList<GenericAttribute> attributes)
        {
            throw new NotImplementedException();
        }

        public GenericAttribute GetAttribute(int attributeId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="keyGroup"></param>
        /// <returns></returns>
        public IList<GenericAttribute> GetAttributesForEntity(int entityId, string keyGroup)
        {
            string key = string.Format(CommonDefaults.GENERICATTRIBUTE_KEY, entityId, keyGroup);
            return _cacheManage.Get(key, () =>
             {
                 var result = _genericAttributeRepository.Table.Where
                (ga => ga.EntityId == entityId && ga.KeyGroup == keyGroup).ToList();
                 return result;
             });           
        }

        public void InsertAttribute(GenericAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            _genericAttributeRepository.Insert(attribute);
        }

        /// <summary>
        /// 保存属性值
        /// </summary>
        /// <typeparam name="TPropType">属性类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public virtual void SaveAttribute<TPropType>(BaseEntity entity, string key, TPropType value)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (key == null)
                throw new ArgumentNullException("key");

            string keyGroup = entity.GetUnproxiedEntityType().Name;

            var props = GetAttributesForEntity(entity.Id, keyGroup)
                .ToList();
            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            var valueStr = CommonHelper.To<string>(value);

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(valueStr))
                {
                    //delete
                    DeleteAttribute(prop);
                }
                else
                {
                    //update
                    prop.Value = valueStr;
                    UpdateAttribute(prop);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(valueStr))
                {
                    //insert
                    prop = new GenericAttribute
                    {
                        EntityId = entity.Id,
                        Key = key,
                        KeyGroup = keyGroup,
                        Value = valueStr,

                    };
                    InsertAttribute(prop);
                }
            }
        }

        public void UpdateAttribute(GenericAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            _genericAttributeRepository.Update(attribute);
        }
        #endregion

    }
}
