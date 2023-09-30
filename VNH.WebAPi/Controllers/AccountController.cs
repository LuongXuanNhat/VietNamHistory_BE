using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using VNH.Application.Common.Contants;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.Interfaces.Catalog.IAccountService;
using Microsoft.AspNetCore.Authentication.Facebook;
using System.Security.Claims;

namespace VNH.WebAPi.Controllers
{
    [Route("")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _account;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;
        public AccountController(IAccountService account, IDistributedCache cache, IConfiguration configuration) {
            _account = account;
            _cache = cache;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _account.Authenticate(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            var userPrincipal = _account.ValidateToken(result.ResultObj);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                IsPersistent = true
            };

            HttpContext.Session.SetString(SystemConstants.Token, result.ResultObj);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

            var cacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };
            await _cache.SetStringAsync("my_token_key", SystemConstants.Token, cacheOptions);
            return Ok(result);
        }

        [HttpPost("LoginFacebook")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginFacebook()
        {

            //string appId = _configuration.GetValue<string>("Authentication:Facebook:AppId"); ;
            //string redirectUri = "/Home";
            //string facebookLoginUrl = $"https://www.facebook.com/v10.0/dialog/oauth?client_id={appId}&redirect_uri={redirectUri}";

            //return Redirect(facebookLoginUrl);
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("FacebookReponse")
            };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("FacebookReponse")
            };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet("facebook-response")]
        public async Task<IActionResult> FacebookReponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities
            .FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            return Ok(claims);

        }

        [HttpGet("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return Ok("Đăng xuất thành công");
        }

        [HttpPost("Signup")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
        {
            var result = await _account.Register(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("EmailConfirm")]
        [Authorize]
        public async Task<IActionResult> EmailConfirm(string numberConfirm)
        {
            var result = await _account.EmailConfirm(numberConfirm, User.Identity.Name);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var result = await _account.ForgetPassword(email);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("ForgetPassword/ConfirmCode")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmCode(LoginRequest request)
        {
            var result = await _account.ConfirmCode(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPassDto resetPass)
        {
            var result = await _account.ResetPassword(resetPass);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }
    }
}
