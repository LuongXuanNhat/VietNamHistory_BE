using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common;
using VNH.Application.Services.Catalog.Users;
using VNH.Domain;
using VNH.Infrastructure.Presenters.Migrations;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.DTOs.Common.SendEmail;
using VNH.Application.Interfaces.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using AutoMapper;

namespace VNH.Infrastructure.Implement.Catalog.Users
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        public readonly ISendMailService _sendmailservice;
        public readonly ILogger<UserService> _logger;
        private readonly IConfiguration _config;
        private readonly VietNamHistoryContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        public UserService(VietNamHistoryContext context,
                ILogger<UserService> logger,
                UserManager<User> userManager, 
                SignInManager<User> signInManager,
                IConfiguration configuration,
                ISendMailService sendmailservice,
                IMapper mapper)
        {
            _dataContext = context;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
            _sendmailservice = sendmailservice;
        }
        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return new ApiErrorResult<string>("Tài khoản không tồn tại");
            if (user.LockoutEnabled.Equals(true))
            {
                if (user.AccessFailedCount == -1)
                {
                    return new ApiErrorResult<string>("Tài khoản bị khóa vĩnh viễn");
                }
                return new ApiErrorResult<string>("Tài khoản bị khóa");
            }
            var accessFailedCount = user.AccessFailedCount;
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, true);
            if (accessFailedCount == 4 && !result.Succeeded)
            {
                await LockAccount(user);
                return new ApiErrorResult<string>("Bạn đã nhập sai mật khẩu liên tục 5 lần! Tài khoản của bạn đã bị khóa. Để lấy lại tài khoản vui lòng thực hiện quên mật khẩu");
            }
            if (!result.Succeeded) return new ApiErrorResult<string>("Sai mật khẩu");
            var token = await GetToKen(user);

            return new ApiSuccessResult<string>(token); 
        }

        public async Task<string> GetToKen(User user)
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
        public async Task<ApiResult<ResetPassDTO>> ConfirmCode(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null && user.NumberConfirm.Equals(request.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = new ResetPassDTO()
                {
                    Email = request.Email,
                    Token = token
                };
                return new ApiSuccessResult<ResetPassDTO>(result);
            }
            return new ApiErrorResult<ResetPassDTO>("Mã xác nhận không chính xác");
        }

        [Authorize]
        public async Task<ApiResult<bool>> EmailConfirm(string numberConfirm, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return new ApiErrorResult<bool>("Lỗi");
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
            if(user == null)
            {
                return new ApiErrorResult<LoginRequest>("Không tìm thấy tài khoản");
            }
            if (user.AccessFailedCount == -1)
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
                    LockoutEnabled = true
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    var role = "student";
                    var getUser = await _dataContext.Users.FirstOrDefaultAsync(x => x.Email.Equals(request.Email));
                    if (getUser != null) {
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
            MailContent content = new MailContent
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

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;
            validationParameters.ValidAudience = _config["Tokens:Issuer"];
            validationParameters.ValidIssuer = _config["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }

        public async Task<ApiResult<bool>> ResetPassword(ResetPassDTO resetPass)
        {
            var user = await _userManager.FindByEmailAsync(resetPass.Email);
            if (user == null) return new ApiErrorResult<bool>("Lỗi");
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, resetPass.Token ,resetPass.Password);
            if (!resetPasswordResult.Succeeded)
            {
                _logger.LogError("Xảy ra lỗi trong quá trình xử lý | ", resetPasswordResult.Errors.Select(e => e.Description));
                return new ApiErrorResult<bool>("Lỗi!");
            }
            if (user.AccessFailedCount == 5) {
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

        public async Task<ApiResult<UserInforDTO>> GetUserInfor(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return new ApiErrorResult<UserInforDTO>("Lỗi");
            return new ApiSuccessResult<UserInforDTO>(_mapper.Map<UserInforDTO>(user));
        }

        public async Task<ApiResult<UserInforDTO>> Update(UserInforDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return new ApiErrorResult<UserInforDTO>("Lỗi");
            _mapper.Map(request, user);
            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                var updatedUserDTO = _mapper.Map<UserInforDTO>(user);
                return new ApiSuccessResult<UserInforDTO>(updatedUserDTO);
            }
            else
            {
                return new ApiErrorResult<UserInforDTO>("Lỗi khi cập nhật thông tin người dùng");
            }
        }
    }
}
