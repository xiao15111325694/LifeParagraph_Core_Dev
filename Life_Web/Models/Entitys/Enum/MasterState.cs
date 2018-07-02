using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 
    /// </summary>
    public enum MasterState 
    {
        /// <summary>
        /// 火热
        /// </summary>
        Hot =1,
        /// <summary>
        /// 推荐
        /// </summary>
        Recommend = 2,
        /// <summary>
        /// 正常
        /// </summary>
        normal = 3,
        /// <summary>
        /// 今日评论最多
        /// </summary>
        CommentaryTheDay = 4,
        /// <summary>
        /// 本月评论最多
        /// </summary>
        CommentaryTheMonth=5,
    }
}
