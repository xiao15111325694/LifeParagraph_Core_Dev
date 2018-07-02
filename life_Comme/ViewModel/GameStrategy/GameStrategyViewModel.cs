using System;
using System.Collections.Generic;
using System.Text;

namespace life_Comme
{
    public class GameStrategyViewModel
    {
        /// <summary>
        /// 来源
        /// </summary>
        public string FromUrl { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string GameTitle { get; set; }

        /// <summary>
        /// 阅读数
        /// </summary>
        public int? ReadCount { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int? CommentCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int GameNodeID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GameNodeName { get; set; }
    }
}
