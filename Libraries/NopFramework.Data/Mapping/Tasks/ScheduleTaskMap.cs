﻿using NopFramework.Core.Demains;
namespace NopFramework.Data.Mapping.Tasks
{
    public partial class ScheduleTaskMap : SunEntityTypeConfiguration<ScheduleTask>
    {
        public ScheduleTaskMap()
        {
            this.ToTable("ScheduleTask");
            this.HasKey(t => t.Id);
            this.Property(t => t.Name).IsRequired();
            this.Property(t => t.Type).IsRequired();
        }
    }
}
