using NopFramework.Core.Domains.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Data.Mapping.Logging
{
    public partial class ActivityLogMap : SunEntityTypeConfiguration<ActivityLog>
    {
        public ActivityLogMap()
        {
            this.ToTable("ActivityLog");
            this.HasKey(al => al.Id);
            this.Property(al => al.Comment).IsRequired();
            this.Property(al => al.IpAddress).HasMaxLength(200);

            this.HasRequired(al => al.ActivityLogType).WithMany()
                .HasForeignKey(al => al.ActivityLogTypeId);
            this.HasRequired(al => al.Customer).WithMany()
                .HasForeignKey(al => al.CustomerId);
            
        }
    }
}
