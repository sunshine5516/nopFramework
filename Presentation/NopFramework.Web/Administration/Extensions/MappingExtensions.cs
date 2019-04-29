using NopFramework.Admin.Infrastructure.Mapper;
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
namespace NopFramework.Admin.Extensions
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }
        #region 电子邮件账号Email account
        public static EmailAccountModel ToModel(this EmailAccount entity)
        {
            return entity.MapTo<EmailAccount, EmailAccountModel>();
        }

        public static EmailAccount ToEntity(this EmailAccountModel model)
        {
            return model.MapTo<EmailAccountModel, EmailAccount>();
        }

        public static EmailAccount ToEntity(this EmailAccountModel model, EmailAccount destination)
        {
            return model.MapTo(destination);
        }
        #endregion


        #region 用户
        public static CustomerModel ToModel(this Customer entity)
        {
            return entity.MapTo<Customer, CustomerModel>();
        }
        public static Customer ToEntity(this CustomerModel model, Customer destination)
        {
            return model.MapTo(destination);
        }
        public static Customer ToEntity(this CustomerModel model)
        {
            return model.MapTo<CustomerModel, Customer>();
        }
        #endregion
        #region 电子邮件消息队列Queued email
        public static QueuedEmailModel ToModel(this QueuedEmail entity)
        {
            return entity.MapTo<QueuedEmail, QueuedEmailModel>();
        }
        public static QueuedEmail ToEntity(this QueuedEmailModel model)
        {
            return model.MapTo<QueuedEmailModel, QueuedEmail>();
        }
        public static QueuedEmail ToEntity(this QueuedEmailModel model, QueuedEmail destination)
        {
            return model.MapTo(destination);
        }
        #endregion

        #region 插件Plugins

        public static PluginModel ToModel(this PluginDescriptor entity)
        {
            return entity.MapTo<PluginDescriptor, PluginModel>();
        }

        #endregion

        #region 插件小窗口Widgets
        public static WidgetModel ToModel(this IWidgetPlugin entity)
        {
            return entity.MapTo<IWidgetPlugin, WidgetModel>();
        }
        #endregion
        #region 系统角色
        //customer roles
        public static CustomerRoleModel ToModel(this CustomerRole entity)
        {
            return entity.MapTo<CustomerRole, CustomerRoleModel>();
        }

        public static CustomerRole ToEntity(this CustomerRoleModel model)
        {
            return model.MapTo<CustomerRoleModel, CustomerRole>();
        }

        public static CustomerRole ToEntity(this CustomerRoleModel model, CustomerRole destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region 日志类型
        public static ActivityLogTypeModel ToModel(this ActivityLogType entity)
        {
            return entity.MapTo<ActivityLogType, ActivityLogTypeModel>();
        }

        #endregion
        #region 活动日志
        public static ActivityLogModel ToModel(this ActivityLog entity)
        {
            return entity.MapTo<ActivityLog, ActivityLogModel>();
        }
        public static ActivityLog ToEntity(this ActivityLogModel model, ActivityLog destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region 新闻
        public static NewsItemModel ToModel(this NewsItem entity)
        {
            return entity.MapTo<NewsItem, NewsItemModel>();
        }
        public static NewsItem ToEntity(this NewsItemModel model, NewsItem destination)
        {
            return model.MapTo(destination);
        }
        public static NewsItem ToEntity(this NewsItemModel model)
        {
            return model.MapTo<NewsItemModel, NewsItem>();
        }
        #endregion
        #region 商品管理
        public static ProductModel ToModel(this Product entity)
        {
            return entity.MapTo<Product, ProductModel>();
        }
        public static Product ToEntity(this ProductModel model, Product destination)
        {
            return model.MapTo(destination);
        }
        public static Product ToEntity(this ProductModel model)
        {
            return model.MapTo<ProductModel, Product>();
        }
        #endregion



        #region 词条管理
        public static TermModel ToModel(this Term entity)
        {
            return entity.MapTo<Term, TermModel>();
        }
        public static Term ToEntity(this TermModel model, Term destination)
        {
            return model.MapTo(destination);
        }
        public static Term ToEntity(this TermModel model)
        {
            return model.MapTo<TermModel, Term>();
        }
        #endregion
        #region 第三方授权
        public static AuthenticationMethodModel ToModel(this IExternalAuthenticationMethod entity)
        {
            return entity.MapTo<IExternalAuthenticationMethod, AuthenticationMethodModel>();
        }
        #endregion
        #region 语言
        public static LanguageModel ToModel(this Language entity)
        {
            return entity.MapTo<Language, LanguageModel>();
        }
        public static Language ToEntity(this LanguageModel model)
        {
            return model.MapTo<LanguageModel, Language>();
        }

        public static Language ToEntity(this LanguageModel model, Language destination)
        {
            return model.MapTo(destination);
        }
        #endregion

    }
}