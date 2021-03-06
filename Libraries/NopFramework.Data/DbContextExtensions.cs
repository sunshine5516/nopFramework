﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Data
{
    public static class DbContextExtensions
    {
        public static IDictionary<string, int> GetColumnsMaxLength(this IDbContext context, string entityTypeName, params string[] columnNames)
        {
            var entType = Type.GetType(entityTypeName);
            var adapter = ((IObjectContextAdapter)context).ObjectContext;
            var metadataWorkspace = adapter.MetadataWorkspace;
            var query=from meta in metadataWorkspace.GetItems(DataSpace.CSpace).Where(m => m.BuiltInTypeKind == BuiltInTypeKind.EntityType)
                      from p in (meta as EntityType).Properties.Where(p => columnNames.Contains(p.Name) && p.TypeUsage.EdmType.Name == "String")
                      select p;
            int temp;
            var queryResult = query.Where(p =>
            {
                var match = p.DeclaringType.Name == entityTypeName;
                if (!match && entType != null)
                {
                    //Is a fully qualified name....
                    match = entType.Name == p.DeclaringType.Name;
                }

                return match;

            }).Select(sel => new { sel.Name, MaxLength = sel.TypeUsage.Facets["MaxLength"].Value }).Where(p => Int32.TryParse(p.MaxLength.ToString(), out temp)).ToDictionary(p => p.Name, p => Convert.ToInt32(p.MaxLength));

            return queryResult;
        }
    }
}
