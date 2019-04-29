using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Media
{
    public class Resources : BaseEntity
    {
        /// <summary>
        /// 资料二进制
        /// </summary>
        public byte[] ResourcesBinary { get; set; }

        /// <summary>
        /// MIME类型
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// SEO友好的文件名
        /// </summary>
        public string SeoFilename { get; set; }
        public string ThumbName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        //public string UrlPath { get; set; }
        /// <summary>
        /// 图片是否是新的
        /// </summary>
        public bool IsNew { get; set; }
        public string ResourceType { get; set; }
    }
}
