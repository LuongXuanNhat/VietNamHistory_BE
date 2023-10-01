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
using Microsoft.AspNetCore.Identity;
using VNH.Domain;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace VNH.WebAPi.Controllers
{
    [Route("")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _account;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
      //  private readonly ILogger<ExternalLoginModel> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;


        public AccountController(IAccountService account, IDistributedCache cache, 
                IConfiguration configuration, SignInManager<User> signInManager,
                UserManager<User> userManager, IEmailSender emailSender)
        {
            _account = account;
            _cache = cache;
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
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

        [HttpPost("LoginGoogle")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginGoogle()
        {
            //var properties = new AuthenticationProperties
            //{
            //    RedirectUri = Url.Action("LoginExpand")
            //};
            //return Challenge(properties, GoogleDefaults.AuthenticationScheme);

            var provider = "Google";

            var redirectUrl = "/api/ExternalLogin/CallBack";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }
        [HttpGet]
        [Route("CallBack")]
        public async Task<IActionResult> CallBack(string remoteError = null)
        {
            var returnUrl = Url.Content("~/");
            if (remoteError != null)
            {
                //ErrorMessage = $"Error from external provider: {remoteError}";
                return Redirect("/Identity/Account/Login");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                //ErrorMessage = "Error loading external login information.";
                return Redirect("/Identity/Account/Login");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
              //  _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return Redirect("/");
            }
            if (result.IsLockedOut)
            {
                return Redirect("/Identity/Account/Login");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                //ReturnUrl = returnUrl;
                //LoginProvider = info.LoginProvider;
                var Email = "";
                var Name = info.Principal.Identity.Name;

                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email);
                }

                var user = new User { UserName = Email, Email = Name };
                var result2 = await _userManager.CreateAsync(user);
                if (result2.Succeeded)
                {
                    result2 = await _userManager.AddLoginAsync(user, info);
                    if (result2.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                     //   _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        return Redirect("/");
                    }
                }
                foreach (var error in result2.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Redirect("/");
            }
        }

        [HttpPost("LoginFacebook")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginFacebook()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("LoginExpand")
            };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("LoginExpand")
            };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet("login-expand-response")]
        public async Task<IActionResult> LoginExpand()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault()
                .Claims.Select(claim => new
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
