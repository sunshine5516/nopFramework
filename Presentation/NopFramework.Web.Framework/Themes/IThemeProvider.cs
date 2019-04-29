using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Web.Framework.Themes
{
    public partial interface IThemeProvider
    {
        /// <summary>
        /// 获取指定主题配置信息
        /// </summary>
        /// <param name="themeName"></param>
        /// <returns></returns>
        ThemeConfiguration GetThemeConfiguration(string themeName);
        /// <summary>
        /// 获取全部主题的配置信息集合
        /// </summary>
        /// <returns></returns>
        IList<ThemeConfiguration> GetThemeConfigurations();
        /// <summary>
        /// 判断主题是否存在
        /// </summary>
        /// <param name="themeName"></param>
        /// <returns></returns>
        bool ThemeConfigurationExists(string themeName);
    }
}
