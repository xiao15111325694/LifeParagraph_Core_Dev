using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Life_Untiy;
using Life_Web.Life_Server;
using Life_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Life_Web.Areas.Life.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    public class TokenController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly JwtSecurityConfig _config;
        private readonly ILogger _logger;
        private readonly IUserServer _userServer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="logger"></param>
        /// <param name="userServer"></param>
        public TokenController(IOptions<JwtSecurityConfig> option,
           ILogger<TokenController> logger,
            IUserServer userServer)
        {
            _config = option.Value;
            _logger = logger;
            _userServer = userServer;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [Route("api/token")]
        [HttpPost]
        public async Task<IActionResult> CreateJwt(string userEmail, string password)
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
                new Claim(ClaimTypes.Name,result.UserName), 
                new Claim(JwtRegisteredClaimNames.Email, result.Email), 
                new Claim(ClaimTypes.NameIdentifier,result.Id.ToString()),
                 };

                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.JwtSecurityKey)); //授权密匙
                var token = new JwtSecurityToken(
                             issuer: _config.Issuer,  //授权签发者
                             audience: _config.Audience, //授权签收者
                             claims: claims,  
                             expires: DateTime.Now.AddMinutes(5), //过期时间
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