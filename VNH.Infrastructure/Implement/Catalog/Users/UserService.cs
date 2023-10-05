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
using VNH.Application.Interfaces.Common;

namespace VNH.Infrastructure.Implement.Catalog.Users
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IImageService _image;
        public readonly ISendMailService _sendmailservice;
        public readonly ILogger<UserService> _logger;
        private readonly UserManager<User> _userManager;


        public UserService(VietNamHistoryContext context,
                ILogger<UserService> logger,
                UserManager<User> userManager,
                SignInManager<User> signInManager,
                IConfiguration configuration,
                ISendMailService sendmailservice,
                IMapper mapper, IImageService image)
        {
            _logger = logger;
            _mapper = mapper;
            _image = image;
            _userManager = userManager;
            _sendmailservice = sendmailservice;
        }
        public async Task<ApiResult<UserDetailDto>> GetUserDetail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return new ApiErrorResult<UserDetailDto>("Lỗi");
            var userDetail = _mapper.Map<UserDetailDto>(user);
            userDetail.Image = _image.ConvertByteArrayToString(user.Image, Encoding.UTF8);
            return new ApiSuccessResult<UserDetailDto>(userDetail);
        }

        public async Task<ApiResult<UserDetailDto>> Update(UserUpdateDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            _mapper.Map(request, user);
            user.Image = (request.Image is not null) ? await _image.ConvertFormFileToByteArray(request.Image) : null;

            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                var userDetail = _mapper.Map<UserDetailDto>(user);
                userDetail.Image = _image.ConvertByteArrayToString(user.Image, Encoding.UTF8);
                return new ApiSuccessResult<UserDetailDto>(userDetail);
            }
            else
            {
                return new ApiErrorResult<UserDetailDto>("Lỗi khi cập nhật thông tin người dùng");
            }
        }
    }
}
