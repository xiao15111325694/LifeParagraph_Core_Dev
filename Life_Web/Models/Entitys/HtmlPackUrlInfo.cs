using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class HtmlPackUrlInfo : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public string HtmlUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? NodeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HtmlContextName { get; set; }
    } 
}
