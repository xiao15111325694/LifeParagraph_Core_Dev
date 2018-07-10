using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Life_Web.Models;
using Life_Web.Models.AccountViewModels;
using Life_Web.Services;
using Models;
using Life_Web.Life_Server;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Life_Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUserServer _userServer;

        public AccountController(
            ILogger<AccountController> logger,
            IUserServer userServer)
        {
            _logger = logger;
            _userServer = userServer;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServer.CheckEmailAndPasswordAsync(model.Email, model.Password);

                if (result == Models.SignInResult.Success)
                {
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    identity.AddClaim(new Claim(ClaimTypes.Name, model.Email));
                                    identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                   
                    //验证是否授权成功
                    if (principal.Identity.IsAuthenticated)
                    {
                        return RedirectToAction("Home/Index");
                    }
                    _logger.LogInformation("User logged in.");
                   
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "用户名或密码错误！");
                    _logger.LogError("用户{userName}登录时发生错误！", model.Email);
                    return View();
                }
            }
            // If we got this far, something failed, redisplay form
            return RedirectToLocal(returnUrl);  
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var userInfo = new User { UserName = model.Email, Email = model.Email };
                var result= await _userServer.CreateUserAsync(userInfo, model.Password);
              
                if (result==RegisterResult.Success)
                {
                    _logger.LogInformation("User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
