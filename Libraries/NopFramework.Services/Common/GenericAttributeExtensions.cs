using NopFramework.Core;
using NopFramework.Core.Infrastructure;
using NopFramework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Common
{
    public static class GenericAttributeExtensions
    {
        /// <summary>
        /// 获取实体属性
        /// </summary>
        /// <typeparam name="TPropType">类型</typeparam>
        /// <param name="entity"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TPropType GetAttribute<TPropType>(this BaseEntity entity, string key)
        {
            var genericAttributeService = EngineContext.Current.Resolve<IGenericAttributeService>();
            return GetAttribute<TPropType>(entity, key, genericAttributeService);
        }

        public static TPropType GetAttribute<TPropType>(this BaseEntity entity, string key, 
            IGenericAttributeService genericAttributeService)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            string keyGroup = entity.GetUnproxiedEntityType().Name;
            var props = genericAttributeService.GetAttributesForEntity(entity.Id, keyGroup);
            if (props == null)
                return default(TPropType);
            if (!props.Any())
                return default(TPropType);
            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            if (prop == null || string.IsNullOrEmpty(prop.Value))
                return default(TPropType);

            return CommonHelper.To<TPropType>(prop.Value);
        }
    }
}
