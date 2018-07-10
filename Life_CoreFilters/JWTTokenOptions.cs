using Microsoft.IdentityModel.Tokens;
using System;

namespace Life_CoreFilters
{
    /// <summary>
    /// 
    /// </summary>
    public class JWTTokenOptions
    {

        /// <summary>
        /// 请求路径
        /// </summary>
        public string Path { get; set; } = "/Api/Token";

        /// <summary>
        /// 认证方
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 使用方
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(5000);

        public SigningCredentials SigningCredentials { get; set; }
    }

}
