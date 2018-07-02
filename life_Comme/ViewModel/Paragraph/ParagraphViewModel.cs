using System;
using System.Collections.Generic;
using System.Text;

namespace life_Comme
{
    public class ParagraphViewModel
    {
        public int ID { get; set; }
        public int NodeId { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImgUrl { get; set; }
        public string VidUrl { get; set; }
        public int? ThumbsupCount { get; set; }
        public int? CommentCount { get; set; }
        public int Type { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
   
   
}
