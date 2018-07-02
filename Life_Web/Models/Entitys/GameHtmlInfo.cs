using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GameHtmlInfo:BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public string HtmlUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? GameNodeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HtmlContextName { get; set; }
    }
}
