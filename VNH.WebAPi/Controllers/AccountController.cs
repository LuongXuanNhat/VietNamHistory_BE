using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using VNH.Application.Common.Contants;
using VNH.Application.DTOs.Catalog.Users;
using Microsoft.AspNetCore.Authentication.Google;
using VNH.Application.Interfaces.Catalog.Accounts;
using Facebook;
using VNH.WebAPi.ViewModels.Catalog;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace VNH.WebAPi.Controllers
{
    [Route("")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _account;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;


        public AccountController(IAccountService account, IDistributedCache cache, IConfiguration configuration)
        {
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
                return Ok(result);
            }
            var userPrincipal = _account.ValidateToken(result.ResultObj);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(15),
                IsPersistent = true
            };

            HttpContext.Session.SetString(SystemConstants.Token, result.ResultObj);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

            var cacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12)
            };
            await _cache.SetStringAsync("my_token_key", SystemConstants.Token, cacheOptions);
            return Ok(result);
        }
        [HttpGet("LoginFacebook")]
        public IActionResult LoginFacebook()
        {
            var redirectUri = _configuration["redirectUriFb"];
            var facebookAuthUrl = $"https://www.facebook.com/v18.0/dialog/oauth?client_id=885411713156137&scope=email&response_type=code&redirect_uri={Uri.EscapeDataString(redirectUri)}&state=12345agd";

            return Redirect(facebookAuthUrl);
        }

        [HttpGet("FacebookCallback")]
        public async Task<IActionResult> FacebookCallback()
        {
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



        [HttpGet("LoginGoogle")]
        [AllowAnonymous]
        public IActionResult LoginGoogle()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleCallback")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("GoogleCallback")]
        public async Task<IActionResult> GoogleCallback(string code)
        {
            var redirect_uri = _configuration["redirectUriGg"];
            // Đổi mã xác thực lấy access token
            var tokenResult = await ExchangeCodeForAccessTokenAsync(code, redirect_uri , "750387254646-70b6gucofhsbe9deuk6rc11qutb3i8go.apps.googleusercontent.com", "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");

            // Lấy thông tin cơ bản của người dùng từ Google bằng access token
            var basicUserInfo = await GetBasicUserInfoAsync(tokenResult.AccessToken);

            if (basicUserInfo.IsSuccessful)
            {
                // Lưu thông tin cơ bản vào session
                HttpContext.Session.SetString("UserEmail", basicUserInfo.Email);
                HttpContext.Session.SetString("UserName", basicUserInfo.Name);

                return RedirectToAction("Home");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        private async Task<BasicUserInfoResult> GetBasicUserInfoAsync(object accessToken)
        {
            try
            {
                // Tạo đối tượng HttpClient để gửi yêu cầu HTTP GET
                using var httpClient = new HttpClient();
                // Đặt đường dẫn API Google để lấy thông tin cơ bản của người dùng
                var googleApiUrl = _configuration["googleApiUrlUser"];

                // Đặt thông số truy vấn, bao gồm access token
                var queryString = $"access_token={accessToken}";
                var requestUrl = $"{googleApiUrl}?{queryString}";

                // Gửi yêu cầu HTTP GET
                var response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Đọc phản hồi và chuyển đổi thành đối tượng BasicUserInfoResult
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var basicUserInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<BasicUserInfoResult>(responseContent);
                    basicUserInfo.IsSuccessful = true;
                    return basicUserInfo;
                }
                else
                {
                    return new BasicUserInfoResult
                    {
                        IsSuccessful = false,
                        ErrorMessage = "Không thể lấy thông tin cơ bản từ Google."
                    };
                }
            }
            catch (Exception ex)
            {
                return new BasicUserInfoResult
                {
                    IsSuccessful = false,
                    ErrorMessage = $"Lỗi: {ex.Message}"
                };
            }
        }

        private async Task<OAuthTokenResponse?> ExchangeCodeForAccessTokenAsync(string code, string redirectUri, string clientId, string clientSecret)
        {
            try
            {
                // Tạo đối tượng HttpClient để gửi yêu cầu HTTP POST
                using var httpClient = new HttpClient();
                // Đặt đường dẫn API Google để trao đổi mã code
                var tokenUrlOfGoogle = _configuration["tokenUrlOfGoogle"];

                // Đặt thông số truy vấn, bao gồm mã code, redirect URI, clientId và clientSecret
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("grant_type", "authorization_code")
                });

                // Gửi yêu cầu HTTP POST
                var response = await httpClient.PostAsync(tokenUrlOfGoogle, content);

                // Kiểm tra nếu yêu cầu thành công
                if (response.IsSuccessStatusCode)
                {
                    // Đọc phản hồi và chuyển đổi thành đối tượng OAuthTokenResponse
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<OAuthTokenResponse>(responseContent);
                    return tokenResponse;
                }
                else
                {
                    // Xử lý lỗi nếu có
                    // ...
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }


        [HttpGet("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Remove("Token");
                return Ok(new { message = "Đăng xuất thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("SignUp")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
        {
            var result = await _account.Register(request);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }

        [HttpPost("EmailConfirm")]
        [Authorize]
        public async Task<IActionResult> EmailConfirm(string numberConfirm)
        {
            var result = await _account.EmailConfirm(numberConfirm, User.Identity.Name);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }

        [HttpGet("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromQuery] string email)
        {
            var result = await _account.ForgetPassword(email);
            return Ok(result);
        }

        [HttpGet("ForgetPassword/ConfirmCode")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmCode([FromQuery] string email)
        {
            var result = await _account.ConfirmCode(email);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPassDto resetPass)
        {
            var result = await _account.ResetPassword(resetPass);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);

        }

        [HttpPost("ChangeEmail")]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(string email)
        {
            //// var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var result = await _account.ChangeEmail(User.Identity.Name, email);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChanggPassword(ChangePasswordDto changePasswodDto)
        {
            var result = await _account.ChangePassword(changePasswodDto);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }

        
    }
}
