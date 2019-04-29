using NopFramework.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Data
{
    public static class Extensions
    {
        public static Type GetUnproxiedEntityType(this BaseEntity baseEntity)
        {
            var userType = ObjectContext.GetObjectType(baseEntity.GetType());
            return userType;
        }
    }
}
