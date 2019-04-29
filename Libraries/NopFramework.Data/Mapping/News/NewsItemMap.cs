using NopFramework.Core.Domains.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Data.Mapping.News
{
    public partial class NewsItemMap : SunEntityTypeConfiguration<NewsItem>
    {
        public NewsItemMap()
        {
            this.ToTable("News");
            this.HasKey(n=>n.Id);
            this.Property(ni => ni.Title).IsRequired();
            this.Property(ni => ni.MetaTitle).IsRequired().HasMaxLength(400);
        }
    }
}
