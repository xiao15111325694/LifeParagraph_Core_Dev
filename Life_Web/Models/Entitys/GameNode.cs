using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GameNode:BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public string GameTypeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateTime { get; set; }
    }
}
