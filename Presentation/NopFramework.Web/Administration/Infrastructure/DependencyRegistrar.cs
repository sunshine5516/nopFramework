using Autofac;
using NopFramework.Admin.Factories;
using NopFramework.Core.Configuration;
using NopFramework.Core.Infrastructure;
using NopFramework.Core.Infrastructure.DependencyManagement;
using System;
namespace NopFramework.Admin.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 0; }
        }
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<LanguageModelFactory>().As<ILanguageModelFactory>().InstancePerLifetimeScope();//语言资源服务
        }
    }
}