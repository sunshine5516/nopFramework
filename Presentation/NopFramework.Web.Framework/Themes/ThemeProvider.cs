﻿using NopFramework.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NopFramework.Web.Framework.Themes
{
    /// <summary>
    /// 主题全部集合：项目路径~/Theme/,一个主题一个目录，遍历目录并加载每个主题目录下面的配置信息theme.config
    /// </summary>
    public partial class ThemeProvider : IThemeProvider
    {
        #region 字段
        private readonly IList<ThemeConfiguration> _themeConfigurations = new List<ThemeConfiguration>();
        private readonly string _basePath = string.Empty;
        #endregion
        #region 构造函数
        public ThemeProvider()
        {
            _basePath = CommonHelper.MapPath("~/Theme/");
            LoadConfigurations();
        }
        #endregion
        #region IThemeProvider

        /// <summary>
        /// 获取指定主题配置信息
        /// </summary>
        /// <param name="themeName"></param>
        /// <returns></returns>
        public ThemeConfiguration GetThemeConfiguration(string themeName)
        {
            return _themeConfigurations.SingleOrDefault(x=>x.ThemeName.Equals(themeName, StringComparison.InvariantCultureIgnoreCase));
            
        }
        /// <summary>
        /// 获取全部主题的配置信息集合
        /// </summary>
        /// <returns></returns>
        public IList<ThemeConfiguration> GetThemeConfigurations()
        {
            return _themeConfigurations;
        }
        /// <summary>
        /// 判断主题是否存在
        /// </summary>
        /// <param name="themeName"></param>
        /// <returns></returns>
        public bool ThemeConfigurationExists(string themeName)
        {
            return GetThemeConfigurations().Any(configuration=>configuration.ThemeName.Equals(themeName, StringComparison.InvariantCultureIgnoreCase));
            //throw new NotImplementedException();
        }
        #endregion
        #region 辅助方法
        /// <summary>
        /// 从主题所在根目录开始遍历，并加载这些主题
        /// </summary>
        private void LoadConfigurations()
        {
            foreach (string themeName in Directory.GetDirectories(_basePath))
            {
                var configuration = CreateThemeConfiguration(themeName);
                if (configuration != null)
                {
                    _themeConfigurations.Add(configuration);
                }
            }
        }
        /// <summary>
        /// 读取主题配置文件里的配置信息并封装成一个对象ThemeConfiguration
        /// </summary>
        /// <param name="themePath"></param>
        /// <returns></returns>
        private ThemeConfiguration CreateThemeConfiguration(string themePath)
        {
            var themeDirectory = new DirectoryInfo(themePath);
            var themeConfigFile = new FileInfo(Path.Combine(themeDirectory.FullName, "theme.config"));
            if (themeConfigFile.Exists)
            {
                var doc = new XmlDocument();
                doc.Load(themeConfigFile.FullName);
                return new ThemeConfiguration(themeDirectory.Name, themeDirectory.FullName, doc);
            }
            return null;
        }
        #endregion
    }
}
