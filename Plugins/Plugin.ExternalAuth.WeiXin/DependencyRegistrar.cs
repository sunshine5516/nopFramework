using Autofac;
using Autofac.Core;
using NopFramework.Core.Caching;
using NopFramework.Core.Configuration;
using NopFramework.Core.Infrastructure;
using NopFramework.Core.Infrastructure.DependencyManagement;
using Plugin.ExternalAuth.WeiXin.Core;
using System;
namespace Plugin.ExternalAuth.WeiXin
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 1; }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<WeiXinProviderAuthorizer>().As<IOAuthProviderWeiXinAuthorizer>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static")).InstancePerLifetimeScope() ;
        }
    }
}
