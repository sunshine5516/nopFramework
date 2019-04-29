using NopFramework.Core.Domains.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Data.Mapping.Messages
{
    public partial class ContactMessageMap : SunEntityTypeConfiguration<ContactMessage>
    {
        public ContactMessageMap()
        {
            this.ToTable("ContactMessage");
            this.HasKey(cm => cm.Id);
            this.Property(cm => cm.Name).IsRequired().HasMaxLength(255);
            this.Property(cm => cm.Email).IsRequired().HasMaxLength(255);
            this.Property(cm => cm.Telephone).IsRequired().HasMaxLength(20);
            this.Property(cm => cm.Content).IsRequired().HasMaxLength(1000);
        }
    }
}
