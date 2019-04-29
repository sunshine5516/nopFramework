using NopFramework.Core.Domains.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Messages
{
    public class TermPicture : BaseEntity
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int TermId { get; set; }

        /// <summary>
        /// 图片ID
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public virtual Picture Picture { get; set; }

        /// <summary>
        /// 词条
        /// </summary>
        public virtual Term Term { get; set; }

    }
}
