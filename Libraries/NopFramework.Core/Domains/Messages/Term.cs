using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Messages
{
    public class Term : BaseEntity
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 概述
        /// </summary>
        public string ShortDescription { get; set; }
        public int TermType { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string FullDescription { get; set; }
        public DateTime UpdatedOn { get; set; }

        private ICollection<TermPicture> _productPictures;//图片
        public virtual ICollection<TermPicture> ProductPictures
        {
            get { return _productPictures ?? (_productPictures = new List<TermPicture>()); }
            protected set { _productPictures = value; }
        }
    }
}
