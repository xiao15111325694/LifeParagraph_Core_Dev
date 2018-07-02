using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class ParagraphNode : BaseEntity
    {
        public int ParagraphId { get; set; }
        public int NodeId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
