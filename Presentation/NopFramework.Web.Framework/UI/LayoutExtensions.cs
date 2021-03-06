﻿using NopFramework.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NopFramework.Web.Framework.UI
{

    public static class LayoutExtensions
    {
        #region 脚本元素
        /// <summary>
        /// 附加脚本元素
        /// </summary>
        /// <param name="html"></param>
        /// <param name="part">脚本元素</param>
        /// <param name="excludeFromBundle">是否将此脚本从捆绑中排除</param>
        /// <param name="isAsync">是否为js文件添加属性“async”</param>
        public static void AppendScriptParts(this HtmlHelper html, string part, bool excludeFromBundle = false, bool isAsync = false)
        {
            AppendScriptParts(html, ResourceLocation.Head, part, excludeFromBundle, isAsync);
        }
        public static void AppendScriptParts(this HtmlHelper html, ResourceLocation location, string part, bool excludeFromBundle = false, bool isAsync = false)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AppendScriptParts(location, part, excludeFromBundle, isAsync);
        }

        /// <summary>
        /// 添加脚本元素
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">脚本部分</param>
        /// <param name="excludeFromBundle">是否将此脚本从捆绑中排除的值</param>
        /// <param name="isAsync">指示是否为js文件添加属性“async”的值</param>
        public static void AddScriptParts(this HtmlHelper html, string part, bool excludeFromBundle = false, bool isAsync = false)
        {
            AddScriptParts(html, ResourceLocation.Head, part, excludeFromBundle, isAsync);
        }
        public static void AddScriptParts(this HtmlHelper html, ResourceLocation location, string part, bool excludeFromBundle = false, bool isAsync = false)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AddScriptParts(location, part, excludeFromBundle, isAsync);
        }
        /// <summary>
        /// 生成所有脚本部分
        /// </summary>
        /// <param name="html">HTML帮助类</param>
        /// <param name="urlHelper">URL帮助类</param>
        /// <param name="location">脚本元素的位置</param>
        /// <param name="bundleFiles">是否绑定脚本元素的值</param>
        /// <returns>Generated string</returns>
        public static MvcHtmlString NopFrameworkScripts(this HtmlHelper html, UrlHelper urlHelper,
            ResourceLocation location, bool? bundleFiles = null)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            var demo= MvcHtmlString.Create(pageHeadBuilder.GenerateScripts(urlHelper, location, bundleFiles));
            return MvcHtmlString.Create(pageHeadBuilder.GenerateScripts(urlHelper, location, bundleFiles));
        }
        #endregion
        #region Css元素
        /// <summary>
        /// 附加CSS元素
        /// </summary>
        /// <param name="html"></param>
        /// <param name="part"></param>
        /// <param name="excludeFromBundle"></param>
        public static void AppendCssFileParts(this HtmlHelper html, string part, bool excludeFromBundle = false)
        {
            AppendCssFileParts(html, ResourceLocation.Head, part, excludeFromBundle);
        }
        public static void AppendCssFileParts(this HtmlHelper html, ResourceLocation location, string part, bool excludeFromBundle = false)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AppendCssFileParts(location, part, excludeFromBundle);
        }
        /// <summary>
        /// 添加CSS元素
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">CSS部分</param>
        /// <param name="excludeFromBundle">是否将此脚本从捆绑中排除的值</param>
        public static void AddCssFileParts(this HtmlHelper html, string part, bool excludeFromBundle = false)
        {
            AddCssFileParts(html, ResourceLocation.Head, part, excludeFromBundle);
        }
        /// <summary>
        /// 添加CSS元素
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="location">脚本元素的位置</param>
        /// <param name="part">CSS part</param>
        /// <param name="excludeFromBundle">是否将此脚本从捆绑中排除的值</param>
        public static void AddCssFileParts(this HtmlHelper html, ResourceLocation location, string part, bool excludeFromBundle = false)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AddCssFileParts(location, part, excludeFromBundle);
        }
        /// <summary>
        ///生成所有CSS部件
        /// </summary>
        /// <param name="html">HTML帮助类</param>
        /// <param name="urlHelper">URL帮助类</param>
        /// <param name="location">脚本元素的位置</param>
        /// <param name="bundleFiles">是否绑定脚本元素的值</param>
        /// <returns>生成的字符串</returns>
        public static MvcHtmlString NopFrameworkCssFiles(this HtmlHelper html, UrlHelper urlHelper,
            ResourceLocation location, bool? bundleFiles = null)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            return MvcHtmlString.Create(pageHeadBuilder.GenerateCssFiles(urlHelper, location, bundleFiles));
        }

        #endregion




        /// <summary>
        /// 获取应选择的管理菜单项的系统名称（扩展）
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetActiveMenuItemSystemName(this HtmlHelper html)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            return pageHeadBuilder.GetActiveMenuItemSystemName();
        }
        /// <summary>
        /// 指定应选择的管理菜单项的系统名称（扩展）
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="systemName">System name</param>
        public static void SetActiveMenuItemSystemName(this HtmlHelper html, string systemName)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.SetActiveMenuItemSystemName(systemName);
        }

        /// <summary>
        /// 生成所有自定义元素
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <returns>Generated string</returns>
        public static MvcHtmlString NopFrameworkHeadCustom(this HtmlHelper html)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            return MvcHtmlString.Create(pageHeadBuilder.GenerateHeadCustom());
        }


    }
}
