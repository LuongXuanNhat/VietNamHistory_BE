using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.DTOs.Common.SendEmail;
using VNH.Application.Interfaces.Catalog.Accounts;
using VNH.Application.Interfaces.Email;
using VNH.Domain;
using VNH.Infrastructure.Implement.Catalog.Users;
using VNH.Infrastructure.Presenters.Migrations;

namespace VNH.Infrastructure.Implement.Catalog.Account
{
    public class AccountService : IAccountService
    {
        public readonly ISendMailService _sendmailservice;
        public readonly ILogger<UserService> _logger;
        private readonly IConfiguration _config;
        private readonly VietNamHistoryContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        public AccountService(VietNamHistoryContext context,
                ILogger<UserService> logger,
                UserManager<User> userManager,
                SignInManager<User> signInManager,
                IConfiguration configuration,
                ISendMailService sendmailservice)
        {
            _dataContext = context;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
            _sendmailservice = sendmailservice;
        }
        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            var errorMessages = new Dictionary<Func<bool>, string>
            {
                { () => user == null, "Tài khoản không tồn tại" },
                { () => user.LockoutEnabled && user.AccessFailedCount == -1, "Tài khoản bị khóa vĩnh viễn" },
                { () => user.LockoutEnabled, "Tài khoản bị khóa" },
                { () => true, "ok" },
            };
            var errorMessage = errorMessages.First(kv => kv.Key()).Value;
            if (!errorMessage.Equals("ok"))
            {
                return new ApiErrorResult<string>(errorMessage);
            }

            var accessFailedCount = user.AccessFailedCount;
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, true);

            if (!result.Succeeded)
            {
                if (accessFailedCount is 4)
                {
                    await LockAccount(user);
                    return new ApiErrorResult<string>("Bạn đã nhập sai mật khẩu liên tục 5 lần! Tài khoản của bạn đã bị khóa. Để lấy lại tài khoản vui lòng thực hiện quên mật khẩu");
                }
                return new ApiErrorResult<string>("Sai mật khẩu");
            }
            return new ApiSuccessResult<string>(await GetToken(user));
        }

        public async Task<string> GetToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(ClaimTypes.Role, string.Join(";",roles)),
            new Claim(ClaimTypes.Name, user.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // Confirm Code to Reset Password
        public async Task<ApiResult<ResetPassDto>> ConfirmCode(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user != null && user.NumberConfirm.Equals(loginRequest.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = new ResetPassDto()
                {
                    Email = loginRequest.Email,
                    Token = token
                };
                return new ApiSuccessResult<ResetPassDto>(result);
            }
            return new ApiErrorResult<ResetPassDto>("Mã xác nhận không chính xác");
        }

        [Authorize]
        public async Task<ApiResult<bool>> EmailConfirm(string numberConfirm, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return new ApiErrorResult<bool>("Lỗi");
            if (user.NumberConfirm.Equals(numberConfirm))
            {
                user.EmailConfirmed = true;
                _dataContext.User.Update(user);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Xác thực không thành công");
        }

        public async Task<ApiResult<LoginRequest>> ForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return new ApiErrorResult<LoginRequest>("Không tìm thấy tài khoản");
            }
            if (user.AccessFailedCount is -1)
            {
                return new ApiErrorResult<LoginRequest>("Tài khoản bị khóa vĩnh viễn");
            }
            var confirmNumber = GetConfirmCode();
            user.NumberConfirm = confirmNumber;
            _dataContext.User.Update(user);
            await _dataContext.SaveChangesAsync();

            await SendConfirmCodeToEmail(user.Email, confirmNumber);

            var result = new LoginRequest()
            {
                Email = email
            };

            return new ApiSuccessResult<LoginRequest>(result);
        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user != null) return new ApiErrorResult<bool>("Email đã tồn tại");
                var confirmNumber = GetConfirmCode();
                user = new User()
                {
                    Id = Guid.NewGuid(),
                    UserName = request.Email,
                    Email = request.Email,
                    NumberConfirm = confirmNumber,
                    LockoutEnabled = false
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    var role = "student";
                    var getUser = await _dataContext.Users.FirstOrDefaultAsync(x => x.Email.Equals(request.Email));
                    if (getUser != null)
                    {
                        await SendConfirmCodeToEmail(request.Email, confirmNumber);

                        await _userManager.AddToRoleAsync(getUser, role);
                        return new ApiSuccessResult<bool>();
                    }
                }

                return new ApiErrorResult<bool>("Đăng ký không thành công : Mật khẩu không hợp lệ, yêu cầu gồm có ít 6 ký tự bao gồm ký tự: Hoa, thường, số, ký tự đặc biệt ");
            }
            catch (Exception ex)
            {
                _logger.LogError("Xảy ra lỗi trong quá trình đăng ký | ", ex.Message);
                return new ApiErrorResult<bool>("Lỗi đăng ký!");
            }
        }

        private async Task SendConfirmCodeToEmail(string email, string confirmNumber)
        {
            MailContent content = new()
            {
                To = email,
                Subject = "Yêu cầu xác nhận email từ [Người Kể Sử]",
                Body = "Xin chào " + email + " , <p> Chúng tôi đã nhận yêu cầu xác thực tài khoản web [NguoiKeSu] của bạn. <p> Mã dùng một lần của bạn là: <strong>" + confirmNumber + "</strong>"
            };

            await _sendmailservice.SendMail(content);
        }

        private string GetConfirmCode()
        {
            return new Random().Next(10000, 100000).ToString();
        }

        public ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;
            TokenValidationParameters validationParameters = new()
            {
                ValidateLifetime = true,
                ValidAudience = _config["Tokens:Issuer"],
                ValidIssuer = _config["Tokens:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
            };

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);

            return principal;
        }

        public async Task<ApiResult<bool>> ResetPassword(ResetPassDto resetPass)
        {
            var user = await _userManager.FindByEmailAsync(resetPass.Email);
            if (user is null) return new ApiErrorResult<bool>("Lỗi");
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, resetPass.Token, resetPass.Password);
            if (!resetPasswordResult.Succeeded)
            {
                _logger.LogError("Xảy ra lỗi trong quá trình xử lý | ", resetPasswordResult.Errors.Select(e => e.Description));
                return new ApiErrorResult<bool>("Lỗi!");
            }
            if (user.AccessFailedCount is 5)
            {
                user.LockoutEnabled = false;
                user.AccessFailedCount = 0;
                _dataContext.User.Update(user);
                await _dataContext.SaveChangesAsync();
            }
            return new ApiSuccessResult<bool>();
        }

        public async Task LockAccount(User user)
        {
            user.LockoutEnabled = true;
            user.AccessFailedCount = 5;
            _dataContext.User.Update(user);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<ApiResult<bool>> ChangePassword(ChangePasswordDto changePasswodDto)
        {
            var user = await _userManager.FindByEmailAsync(changePasswodDto.Email);
            var passwordCheckResult = await _userManager.CheckPasswordAsync(user, changePasswodDto.Password);
            if (!passwordCheckResult)
            {
            return new ApiErrorResult<bool>("Mật khẩu hiện tại không đúng");
            } 
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePasswodDto.Password, changePasswodDto.NewPassword);
            if (changePasswordResult.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Đổi mật khẩu không thành công");
        }

        public async Task<ApiResult<string>> LoginExtend(string email, string name)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                var newAccount = new User { UserName = email, Email = email };
                var createdResult = await _userManager.CreateAsync(newAccount);
                return createdResult.Succeeded ? new ApiSuccessResult<string>(await GetToken(newAccount)) : new ApiErrorResult<string>("Lỗi đăng nhập, hiện tại chưa xử lý được");
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return new ApiSuccessResult<string>(await GetToken(user));
        }
    }
}
