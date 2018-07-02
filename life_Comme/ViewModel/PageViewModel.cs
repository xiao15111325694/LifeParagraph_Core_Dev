using System;
using System.Collections.Generic;
using System.Text;

namespace Life_Paragraph_Core
{
    public class PageViewModel<T>
    {
        public List<T> ViewModel { get; set; }
      

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }

    }
}
