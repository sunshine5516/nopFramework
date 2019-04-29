using AutoMapper;
using NopFramework.Admin.Models.Catalog;
using NopFramework.Admin.Models.Cms;
using NopFramework.Admin.Models.Customers;
using NopFramework.Admin.Models.ExternalAuthentication;
using NopFramework.Admin.Models.Localization;
using NopFramework.Admin.Models.Logging;
using NopFramework.Admin.Models.Messages;
using NopFramework.Admin.Models.News;
using NopFramework.Admin.Models.Plugins;
using NopFramework.Admin.Models.Term;
using NopFramework.Core.Domains.Catalog;
using NopFramework.Core.Domains.Customers;
using NopFramework.Core.Domains.Localization;
using NopFramework.Core.Domains.Logging;
using NopFramework.Core.Domains.Messages;
using NopFramework.Core.Domains.News;
using NopFramework.Core.Plugins;
using NopFramework.Services.Authentication.External;
using NopFramework.Services.Cms;
namespace NopFramework.Admin.Infrastructure.Mapper
{
    /// <summary>
    /// 映射器配置
    /// </summary>
    public static class AutoMapperConfiguration
    {
        private static MapperConfiguration _mapperConfiguration;
        private static IMapper _mapper;
        public static void Init()
        {
            //AutoMapper.Mapper.Initialize
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                //Mapper.CreateMap的第一个参数是源类型，第二参数是目标类型。
                cfg.CreateMap<EmailAccount, EmailAccountModel>()
                    .ForMember(dest => dest.Password, mo => mo.Ignore())
                    .ForMember(dest => dest.IsDefaultEmailAccount, mo => mo.Ignore())
                    .ForMember(dest => dest.SendTestEmailTo, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<EmailAccountModel, EmailAccount>()
                    .ForMember(dest => dest.Password, mo => mo.Ignore());
                //电子邮件队列
                cfg.CreateMap<QueuedEmail, QueuedEmailModel>()
                .ForMember(dest => dest.EmailAccountName,
                        mo => mo.MapFrom(src => src.EmailAccount != null ? src.EmailAccount.FriendlyName : string.Empty))
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.PriorityName, mo => mo.Ignore())
                    .ForMember(dest => dest.DontSendBeforeDate, mo => mo.Ignore())
                    .ForMember(dest => dest.SendImmediately, mo => mo.Ignore())
                    .ForMember(dest => dest.SentOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<QueuedEmailModel, QueuedEmail>()
                    .ForMember(dest => dest.Priority, dt => dt.Ignore())
                    .ForMember(dest => dest.PriorityId, dt => dt.Ignore())
                    .ForMember(dest => dest.CreatedOn, dt => dt.Ignore())
                    .ForMember(dest => dest.DontSendBeforeDateUtc, mo => mo.Ignore())
                    .ForMember(dest => dest.SentOn, mo => mo.Ignore())
                    .ForMember(dest => dest.EmailAccount, mo => mo.Ignore())
                    .ForMember(dest => dest.EmailAccountId, mo => mo.Ignore())
                    .ForMember(dest => dest.AttachmentFilePath, mo => mo.Ignore())
                    .ForMember(dest => dest.AttachmentFileName, mo => mo.Ignore())
                    .ForMember(dest => dest.AttachedDownloadId, mo => mo.Ignore());
                //插件
                cfg.CreateMap<PluginDescriptor, PluginModel>()
                    .ForMember(dest => dest.ConfigurationUrl, mo => mo.Ignore())
                    .ForMember(dest => dest.CanChangeEnabled, mo => mo.Ignore())
                    .ForMember(dest => dest.IsEnabled, mo => mo.Ignore())
                    .ForMember(dest => dest.LogoUrl, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                //插件小窗口
                cfg.CreateMap<IWidgetPlugin, WidgetModel>()
                    .ForMember(dest => dest.FriendlyName, mo => mo.MapFrom(src => src.PluginDescriptor.FriendlyName))
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.PluginDescriptor.SystemName))
                    .ForMember(dest => dest.DisplayOrder, mo => mo.MapFrom(src => src.PluginDescriptor.DisplayOrder))
                    .ForMember(dest => dest.IsActive, mo => mo.Ignore())
                    .ForMember(dest => dest.ConfigurationActionName, mo => mo.Ignore())
                    .ForMember(dest => dest.ConfigurationControllerName, mo => mo.Ignore())
                    .ForMember(dest => dest.ConfigurationRouteValues, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                //用户角色
                cfg.CreateMap<CustomerRole, CustomerRoleModel>()
                   .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<CustomerRoleModel, CustomerRole>()
                    .ForMember(dest => dest.PermissionRecords, mo => mo.Ignore());


                //用户      
                cfg.CreateMap<Customer, CustomerModel>()
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore())
                    .ForMember(dest => dest.City, mo => mo.Ignore())
                    .ForMember(dest => dest.CityEnable, mo => mo.Ignore())
                    .ForMember(dest => dest.Company, mo => mo.Ignore())
                    
                    .ForMember(dest => dest.LastIpAddress, mo => mo.Ignore())
                    .ForMember(dest => dest.Password, mo => mo.Ignore())
                    .ForMember(dest => dest.Active, opts => opts.MapFrom(src => src.IsActive));
                cfg.CreateMap<CustomerModel, Customer>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.IsDeleted, mo => mo.Ignore())
                    .ForMember(dest => dest.Password, mo => mo.Ignore())
                    .ForMember(dest => dest.LastActivityDate, mo => mo.Ignore())
                    .ForMember(dest => dest.IsActive, opts=>opts.MapFrom(src=>src.Active));


                //日志类型
                cfg.CreateMap<ActivityLogType, ActivityLogTypeModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<ActivityLogTypeModel, ActivityLogType>()
                   .ForMember(dest => dest.SystemKeyword, mo => mo.Ignore());
                //用户日志
                cfg.CreateMap<ActivityLog,ActivityLogModel>()
                .ForMember(dest=>dest.ActivityLogTypeName,mo=>mo.MapFrom(src=>src.ActivityLogType.Name))
                .ForMember(dest=>dest.CustomerEmail,Models=>Models.MapFrom(src=>src.Customer.Email))
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                //新闻
                cfg.CreateMap<NewsItemModel, NewsItem>()
                .ForMember(dest => dest.AhthroId, mo => mo.Ignore())
                .ForMember(dest => dest.CreatedOn, mo => mo.Ignore());
                cfg.CreateMap<NewsItem, NewsItemModel>()
                .ForMember(dest => dest.SeName, mo => mo.Ignore())
                .ForMember(dest => dest.CreatedOn, mo => mo.Ignore());

               

                //商品
                cfg.CreateMap<ProductModel, Product>()
               .ForMember(dest => dest.CreatedOn, mo => mo.Ignore());
                cfg.CreateMap<Product, ProductModel>();


                //词条
                cfg.CreateMap<TermModel, Term>()
               .ForMember(dest => dest.CreatedOn, mo => mo.Ignore());
                cfg.CreateMap<Term, TermModel>();
               
                //第三方授权
                cfg.CreateMap<IExternalAuthenticationMethod, AuthenticationMethodModel>()
                   .ForMember(dest => dest.FriendlyName, mo => mo.MapFrom(src => src.PluginDescriptor.FriendlyName))
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.PluginDescriptor.SystemName))
                    .ForMember(dest => dest.DisplayOrder, mo => mo.MapFrom(src => src.PluginDescriptor.DisplayOrder))
                    .ForMember(dest => dest.IsActive, mo => mo.Ignore())
                    .ForMember(dest => dest.ConfigurationActionName, mo => mo.Ignore())
                    .ForMember(dest => dest.ConfigurationControllerName, mo => mo.Ignore())
                    .ForMember(dest => dest.ConfigurationRouteValues, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                //语言
                cfg.CreateMap<Language, LanguageModel>()
                .ForMember(dest => dest.FlagFileNames, mo => mo.Ignore())
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<LanguageModel, Language>()
                    .ForMember(dest => dest.LocaleStringResources, mo => mo.Ignore());

            });
            _mapper = _mapperConfiguration.CreateMapper();
        }
        public static IMapper Mapper
        {
            get
            {
                return _mapper;
            }
        }
        public static MapperConfiguration MapperConfiguration
        {
            get
            {
                return _mapperConfiguration;
            }
        }
    }
}