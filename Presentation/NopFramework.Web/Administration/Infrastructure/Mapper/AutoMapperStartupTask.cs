using NopFramework.Core.Infrastructure;
namespace NopFramework.Admin.Infrastructure.Mapper
{
    public class AutoMapperStartupTask : IStartupTask
    {
        public void Execute()
        {
            AutoMapperConfiguration.Init();
        }

        public int Order
        {
            get { return 0; }
        }
    }
}