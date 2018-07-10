using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Life_Untiy;
using Life_Web.Life_Server;
using Life_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Life_Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    public class JWTOAuthController : Controller
    {
        public JwtSecurityConfig Config;
        private readonly ILogger _logger;
        private readonly IUserServer _userServer;
        public JWTOAuthController(IOptions<JwtSecurityConfig> option,
            ILogger logger,
            IUserServer userServer)
        {
            Config = option.Value;
            _logger = logger;
            _userServer = userServer;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetJwt(string userEmail, string password )
        {
            try
            {
                var result = await _userServer.ReturnCheckEmailAndPasswordAsync(userEmail, password);
                if (result == null)
                {
                    return Unauthorized();
                }
                var claims = new[]
                {
                new Claim(ClaimTypes.Name,result.UserName), //添加用户信息
                new Claim(JwtRegisteredClaimNames.Email, result.Email), // 添加用户邮箱
                new Claim(ClaimTypes.NameIdentifier,result.Id.ToString()),
                 };

                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("Lief_Paragraph_jwtKey-2018-7-4_keyValue"));
                var token = new JwtSecurityToken(
                             issuer: "Life_TestIssuer",
                             audience: "Life-TestAudience",
                             claims: claims,
                             expires: DateTime.Now.AddMinutes(5),
                             signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

                //生成Token
                string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new
                {
                    access_token = jwtToken,
                    token_type = "Bearer",
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("获取访问令牌时发生错误！", ex);
                return Json(new { Error = "授权失败", Code = 401 });
            }

        }

    }
}