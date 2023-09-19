using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common;
using VNH.Application.Services.Catalog.Users;
using VNH.Domain;
using VNH.Infrastructure.Presenters.Migrations;

namespace VNH.Infrastructure.Implement.Catalog.Users
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly VietNamHistoryContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        public UserService(VietNamHistoryContext context,
                UserManager<User> userManager, 
                SignInManager<User> signInManager,
                IConfiguration configuration) {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
        }
        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return new ApiErrorResult<string>("Tài khoản không tồn tại");
            if (user.Status.Equals(true)) return new ApiErrorResult<string>("Tài khoản bị khóa");

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



    }
}
