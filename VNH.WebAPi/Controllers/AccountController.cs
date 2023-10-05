using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using VNH.Application.Common.Contants;
using VNH.Application.DTOs.Catalog.Users;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using VNH.Domain;
using Microsoft.AspNetCore.Authentication.Google;
using VNH.Application.Interfaces.Catalog.Accounts;
using Facebook;
using System.Net;

namespace VNH.WebAPi.Controllers
{
    [Route("")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _account;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;


        public AccountController(IAccountService account, IDistributedCache cache, 
                IConfiguration configuration, SignInManager<User> signInManager,
                UserManager<User> userManager)
        {
            _account = account;
            _cache = cache;
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
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

        [HttpGet("LoginFacebook")]
        public IActionResult LoginFacebook()
        {
            var redirectUri = "https://localhost:7138/FacebookCallback";
            var facebookAuthUrl = $"https://www.facebook.com/v18.0/dialog/oauth?client_id=885411713156137&scope=email&response_type=code&redirect_uri={Uri.EscapeDataString(redirectUri)}&state=12345agd";

            return Redirect(facebookAuthUrl);
        }

        [HttpGet("FacebookCallback")]
        public async Task<IActionResult> FacebookCallback()
        {
           // var result = HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme).Result;
            var code = HttpContext.Request.Query["code"].ToString();
            var state = HttpContext.Request.Query["state"].ToString();

            if (!string.IsNullOrEmpty(code))
            {
                // Lấy access token từ mã truy cập
                var fbClient = new FacebookClient();
                dynamic result = await fbClient.GetTaskAsync(
                    "oauth/access_token",
                    new
                    {
                        client_id = "885411713156137", // Replace with your App ID
                        client_secret = "a10c56a5001e725be954f9504c8c81df", // Replace with your App Secret
                        code = code,
                        redirect_uri = "https://localhost:7138/FacebookCallback"
                    }
                );

                var accessToken = result.access_token;

                // Use the access token to make API calls to Facebook
                fbClient.AccessToken = accessToken;
                dynamic fbUser = await fbClient.GetTaskAsync("me?fields=name,email");

                // Extract user information
                string name = fbUser.name;
                string email = fbUser.email;
                var login = await _account.LoginExtend(email, name);

                return result is not null ? Ok(login) : BadRequest(login);
            }
            return Unauthorized("Đăng nhập không thành công");
        }



        [HttpPost("LoginGoogle")]
        [AllowAnonymous]
        public IActionResult LoginGoogle()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("LoginExpand")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
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

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChanggPassword(ChangePasswordDto changePasswodDto)
        {
            var result = await _account.ChangePassword(changePasswodDto);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
