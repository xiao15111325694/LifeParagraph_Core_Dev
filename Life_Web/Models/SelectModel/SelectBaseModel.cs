using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Life_Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectBaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        public SelectBaseModel()
        {
            if (PageIndex == 0)
            {
                PageIndex = 1;
            }

            if (PageSize == 0)
            {
                PageSize = 10;
            }
           
        }

        /// <summary>
        /// 
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public int PageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pageCount { get; set; }
    }
}
