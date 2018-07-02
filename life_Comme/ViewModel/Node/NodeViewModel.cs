using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace life_Comme
{
    public class NodeViewModel
    {
        public int ID { get; set; }
        [Display(Name ="主题名称")]
        public string NodeName { get; set; }

        [Display(Name = "主题图片")]
        public string NodeImg { get; set; }

        [Display(Name = "主题描述")]
        public string NodeDescribe { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        public int? ParagraphCount { get; set; }
    }
}
