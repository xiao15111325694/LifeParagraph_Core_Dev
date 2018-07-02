using Microsoft.AspNetCore.Identity;
using System;

namespace Models
{
    public class User : BaseEntity
    {

        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string UserName { get; set; }
        public string Title { get; set; }
        public string Profile { get; set; }
        public string Url { get; set; }
        public string GitHub { get; set; }
        public int? PublishCount { get; set; }
        public int? PublishReplyCount { get; set; }
        public int? PointOfpraise { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastTime { get; set; }
    }
}
