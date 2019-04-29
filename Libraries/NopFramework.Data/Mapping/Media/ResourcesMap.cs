using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core.Domains.Media;

namespace NopFramework.Data.Mapping.Media
{
    public partial class ResourcesMap : SunEntityTypeConfiguration<Resources>
    {
        public ResourcesMap()
        {
            this.ToTable("Resources");
            this.HasKey(r => r.Id);
            
        }
    }
}
