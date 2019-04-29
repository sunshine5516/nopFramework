
namespace NopFramework.Services.Localization
{
    public static partial class LocalizationDefaults
    {
        #region 语言
        public static string LANGUAGES_ALL_KEY = "NopFramework.language.all-{0}";
        #endregion
        #region 资源文件
        public static string LocaleStringResourcesAllCacheKey => "NopFramework.lsr.all-{0}";
        public static string LocaleStringResourcesPatternCacheKey => "NopFramework.lsr.";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : resource key
        /// </remarks>
        public static string LOCALSTRINGRESOURCES_BY_RESOURCENAME_KEY = "NopFramework.lsr.{0}-{1}";
        #endregion
    }
}
