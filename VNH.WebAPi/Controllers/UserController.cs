using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using VNH.Application.Common.Contants;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.Services.Catalog.Users;

namespace VNH.WebAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IDistributedCache _cache;

        public UserController(IUserService userService, IDistributedCache cache)
        {
            _cache = cache;
            _userService = userService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _userService.Authenticate(request);

            if (string.IsNullOrEmpty(result.ResultObj))
            {
                return BadRequest(result);
            }
            var userPrincipal = _userService.ValidateToken(result.ResultObj);

            var authProperties = new AuthenticationProperties // Lưu cookie khi vào lại mà không logout
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                IsPersistent = true
            };

            HttpContext.Session.SetString(SystemConstants.Token, result.ResultObj);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

            // Lưu cache token
            var cacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };
            await _cache.SetStringAsync("my_token_key", SystemConstants.Token, cacheOptions);
            return Ok(result);
        }

        [HttpPost("Signup")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
        {
            var result = await _userService.Register(request);
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
            var result = await _userService.EmailConfirm(numberConfirm, User.Identity.Name);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var result = await _userService.ForgetPassword(email);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("ForgetPassword/ConfirmCode")]
        public async Task<IActionResult> ConfirmCode(LoginRequest request)
        {
            var result = await _userService.ConfirmCode(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPassDTO resetPass)
        {
            var result = await _userService.ResetPassword(resetPass);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserInfor()
        {
            var result = await _userService.GetUserInfor(User.Identity.Name);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] UserInforDTO request)
        {
            var result = await _userService.Update(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
