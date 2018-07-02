using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Node:BaseEntity
    {
        public string NodeName { get; set; }

        public string NodeImg { get; set; }

        public string NodeDescribe { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
