using NopFramework.Core.Domains.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Data.Mapping.Messages
{
    public class TermMap : SunEntityTypeConfiguration<Term>
    {
        public TermMap()
        {
            this.ToTable("Term");
            this.HasKey(p => p.Id);
            this.Property(p => p.Name).IsRequired().HasMaxLength(400);
            this.Property(p => p.FullDescription).IsRequired().HasMaxLength(4000);
            //this.Property(p => p.t).IsRequired().HasMaxLength(400);
        }
    }
}
