using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Advertisement : BaseEntity
    {
       public string AdName { get; set; }

        public string CreateTime { get; set; }

        public string Html { get; set; }

        public string LastTime { get; set; }
    } 
}
