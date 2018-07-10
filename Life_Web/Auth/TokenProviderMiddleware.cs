using Life_Core_Repositories;
using Life_CoreFilters;
using Life_Untiy;
using Life_Web.Life_Server;
using Life_Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Life_Web.Auth
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JWTTokenOptions _options;


        public TokenProviderMiddleware(
            RequestDelegate next,
            IOptions<JWTTokenOptions> options,
            IAuthenticationSchemeProvider schemes
            )
        {
            _next = next;
            _options = options.Value;
            Schemes = schemes;
        }
        public IAuthenticationSchemeProvider Schemes { get; set; }


        /// <summary>
        /// invoke 中间件拦截
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            //
            context.Features.Set<IAuthenticationFeature>(new AuthenticationFeature
            {
                OriginalPath = context.Request.Path,
                OriginalPathBase = context.Request.PathBase
            });
            //获取默认Scheme（或者AuthorizeAttribute指定的Scheme）的AuthenticationHandler
            var handlers = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
            {
                var handler = await handlers.GetHandlerAsync(context, scheme.Name) as IAuthenticationRequestHandler;
                if (handler != null && await handler.HandleRequestAsync())
                {
                    return ;
                }
            }
            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                var result = await context.AuthenticateAsync(defaultAuthenticate.Name);
                if (result?.Principal != null)
                {
                    context.User = result.Principal;
                }
            }
            //


            if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
            {
                await _next(context);
                return;
            }
            // Request must be POST with Content-Type: application/x-www-form-urlencoded
            if (!context.Request.Method.Equals("POST")
               || !context.Request.HasFormContentType)
            {
                await ReturnBadRequest(context);
                return;
            }

            await GenerateAuthorizedResult(context);
        }

        /// <summary>
        /// 验证结果并得到token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task GenerateAuthorizedResult(HttpContext context)
        {
            var username = context.Request.Form["username"];
            var password = context.Request.Form["password"];

            var identity = await GetIdentity(username, password);
            if (identity == null)
            {
                await ReturnBadRequest(context);
                return;
            }

            // Serialize and return the response
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(GetJwt(username));
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
              DESHelp dESHelp = new DESHelp();
               var passwordHash = dESHelp.Encrypt(password);
            //var isValidated = await _repository.GetAsync(x => x.Email == username && x.PasswordHash == passwordHash);
            var isValidated = true;
            if (isValidated)
            {
                return await Task.FromResult(new ClaimsIdentity(new System.Security.Principal.GenericIdentity(username, "Token"), new Claim[] { }));
            }
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        /// <summary>
        /// 返回认证失败请求
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task ReturnBadRequest(HttpContext context)
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                Status = false,
                Message = "认证失败"
            }));
        }

        /// <summary>
        /// 生成JWT并返回
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private string GetJwt(string username)
        {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Email, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(),
                          ClaimValueTypes.Integer64),
                //用户名
                new Claim(ClaimTypes.Name,username),
                //角色
                new Claim(ClaimTypes.Role,"admin")
            };

            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_options.Expiration),
                signingCredentials: _options.SigningCredentials
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                Status = true,
                access_token = encodedJwt,
                expires_in = (int)_options.Expiration.TotalSeconds,
                token_type = "Bearer"
            };
            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

    }
}
