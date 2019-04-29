using NopFramework.Core.Domains.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Data.Mapping.Catalog
{
    public class ProductMap: SunEntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            this.ToTable("Product");
            this.HasKey(p => p.Id);
            this.Property(p => p.Name).IsRequired().HasMaxLength(400);
            this.Property(p => p.Parameters).IsRequired().HasMaxLength(400);
            this.Property(p => p.FullDescription).IsRequired().HasMaxLength(4000);
            //this.Property(p => p.t).IsRequired().HasMaxLength(400);
        }
    }
}
