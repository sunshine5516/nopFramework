using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Web.Framework.Themes
{
    public partial class ThemeContext : IThemeContext
    {
        #region 声明实例
        private readonly IThemeProvider _themeProvider;

        private bool _themeIsCached;
        private string _cachedThemeName;
        #endregion

        public ThemeContext(IThemeProvider themeProvider)
        {
            this._themeProvider = themeProvider;
        }
        public string WorkingThemeName
        {

            get
            {
                if (_themeIsCached)
                    return _cachedThemeName;
                string theme = "";

                if (!_themeProvider.ThemeConfigurationExists(theme))
                {
                    //如果不存在默认主题就加载主题集合中的第一个主题
                    var themeInstance = _themeProvider.GetThemeConfigurations().FirstOrDefault();
                    if (themeInstance == null)
                        return null;
                        //throw new Exception("No theme could be loaded");
                    theme = themeInstance.ThemeName;
                }
                this._cachedThemeName = theme;
                this._themeIsCached = true;
                return theme;
            }
            set
            {
                this._themeIsCached = false;
            }
        }
    }
}
