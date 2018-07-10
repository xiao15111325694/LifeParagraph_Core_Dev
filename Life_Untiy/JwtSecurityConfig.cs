using System;
using System.Collections.Generic;
using System.Text;

namespace Life_Untiy
{
    public class JwtSecurityConfig
    {
        /// <summary>
        /// 安全密匙
        /// </summary>
        public string JwtSecurityKey { get; set; }

        /// <summary>
        /// Token签发者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Token签收者
        /// </summary>
        public string Audience { get; set; }

    }
}
