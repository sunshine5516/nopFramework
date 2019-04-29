using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NopFramework.Web.Framework.UI
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IPageHeadBuilder
    {
        void AppendScriptParts(ResourceLocation location, string part, bool excludeFromBundle, bool isAsync);
        void AppendCssFileParts(ResourceLocation location, string part, bool excludeFromBundle = false);
        string GenerateCssFiles(UrlHelper urlHelper, ResourceLocation location, bool? bundleFiles = null);
        string GenerateScripts(UrlHelper urlHelper, ResourceLocation location, bool? bundleFiles = null);
        string GenerateHeadCustom();
        /// <summary>
        /// 指定应选择的管理菜单项的系统名称（扩展）
        /// </summary>
        /// <param name="systemName">System name</param>
        void SetActiveMenuItemSystemName(string systemName);
        /// <summary>
        /// 获取应选择的管理菜单项的系统名称（扩展）
        /// </summary>
        /// <returns>System name</returns>
        string GetActiveMenuItemSystemName();
        /// <summary>
        /// 添加CSS元素
        /// </summary>
        /// <param name="location"></param>
        /// <param name="part"></param>
        /// <param name="excludeFromBundle"></param>
        void AddCssFileParts(ResourceLocation location, string part, bool excludeFromBundle = false);
        /// <summary>
        /// 添加脚本元素
        /// </summary>
        /// <param name="location"></param>
        /// <param name="part"></param>
        /// <param name="excludeFromBundle"></param>
        /// <param name="isAync"></param>
        void AddScriptParts(ResourceLocation location, string part, bool excludeFromBundle, bool isAync);


    }
}
