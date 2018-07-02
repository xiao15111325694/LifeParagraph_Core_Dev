using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class UserCollection :BaseEntity
    {
        public string UserId { get; set; }
        public int TopicId { get; set; }
        public Paragraph Paragraph { get; set; }
        public int State { get; set; }
        public DateTime CreateTime{ get; set; }
    }
}
