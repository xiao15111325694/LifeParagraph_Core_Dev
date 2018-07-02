using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace life_Comme
{
    public class ParagraphAddModel
    {
        [Required]
        public int NodeId { get; set; }
        [Required]
        public string UserId { get; set; }
        public string Email { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string ImgUrl { get; set; }
        public string VidUrl { get; set; }
        public int? ThumbsupCount { get; set; }
        public int? CommentCount { get; set; }
        public int? Type { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
