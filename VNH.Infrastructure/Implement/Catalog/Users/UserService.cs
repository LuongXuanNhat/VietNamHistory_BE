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
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;

namespace VNH.Infrastructure.Implement.Catalog.Users
{
    public class UserService : IUserService
    {
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
            if (user == null) return new ApiErrorResult<string>("Tài khoản không tồn tại");
            if (user.LockoutEnabled.Equals(false)) return new ApiErrorResult<string>("Tài khoản bị khóa");

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, true);
            if (!result.Succeeded) return new ApiErrorResult<string>("Sai mật khẩu");

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(ClaimTypes.Role, string.Join(";",roles)),
            new Claim(ClaimTypes.Name, request.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
            return new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));
            
            
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

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user != null) return new ApiErrorResult<bool>("Email đã tồn tại");
                int confirmNumber = new Random().Next(10000, 100000);

                user = new User()
                {
                    Id = Guid.NewGuid(),
                    UserName = request.Email,
                    Email = request.Email,
                    NumberConfirm = confirmNumber.ToString(),
                    LockoutEnabled = true
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    var role = "student";

                    var getUser = await _dataContext.Users.FirstOrDefaultAsync(x => x.Email.Equals(request.Email));
                    if (getUser != null) {
                        
                        MailContent content = new MailContent
                        {
                            To = request.Email,
                            Subject = "Yêu cầu xác nhận email từ [Người Kể Sử]",
                            Body = "Xin chào " + request.Email + " , <p> Chúng tôi đã nhận yêu cầu xác thực tài khoản web [NguoiKeSu] của bạn. <p> Mã dùng một lần của bạn là: <strong>" + confirmNumber + "</strong>"
                        };

                        await _sendmailservice.SendMail(content);


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
    }
}
