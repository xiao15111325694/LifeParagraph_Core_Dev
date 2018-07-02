using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserMessage:BaseEntity
    {
        /// <summary>
        /// 发送人ID
        /// </summary>
        public string SendUserId { get; set; }
        /// <summary>
        /// 接受人ID
        /// </summary>
        public string ReceiveUserId { get; set; }
        /// <summary>
        /// 评论类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 评论状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime CreateOn { get; set; }
    }
}
