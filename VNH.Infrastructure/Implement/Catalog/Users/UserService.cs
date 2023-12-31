﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.Services.Catalog.Users;
using VNH.Domain;
using VNH.Infrastructure.Presenters.Migrations;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.Interfaces.Email;
using AutoMapper;
using VNH.Application.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using VNH.Infrastructure.Implement.Common;
using Microsoft.EntityFrameworkCore;

namespace VNH.Infrastructure.Implement.Catalog.Users
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IImageService _image;
        public readonly ISendMailService _sendmailservice;
        public readonly ILogger<UserService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly VietNamHistoryContext _dataContext;
        private readonly IStorageService _storageService;

        public UserService(VietNamHistoryContext context,
                ILogger<UserService> logger,
                UserManager<User> userManager,
                SignInManager<User> signInManager,
                IConfiguration configuration,
                ISendMailService sendmailservice,
                IMapper mapper, IImageService image,
                IStorageService storageService)
        {
            _logger = logger;
            _mapper = mapper;
            _image = image;
            _userManager = userManager;
            _sendmailservice = sendmailservice;
            _dataContext = context;
            _storageService = storageService;
        }
        public async Task<ApiResult<UserDetailDto>> GetUserDetail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return new ApiErrorResult<UserDetailDto>("Lỗi");
            var userDetail = _mapper.Map<UserDetailDto>(user);
            return new ApiSuccessResult<UserDetailDto>(userDetail);
        }

        public async Task<ApiResult<UserDetailDto>> Update(UserUpdateDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            _mapper.Map(request, user);

            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                var userDetail = _mapper.Map<UserDetailDto>(user);
                return new ApiSuccessResult<UserDetailDto>(userDetail);
            }
            else
            {
                return new ApiErrorResult<UserDetailDto>("Lỗi khi cập nhật thông tin người dùng");
            }
        }

        public async Task<ApiResult<string>> GetImage(string email)
        {
            var user = await _dataContext.User.FirstOrDefaultAsync(x=>x.Email.Equals(email));
            if(user is null) return new ApiSuccessResult<string>(string.Empty);
            return new ApiSuccessResult<string>(user.Image);
        }

        public async Task<ApiResult<string>> SetImageUser(string email, IFormFile image)
        {
            var user = await _dataContext.User.FirstOrDefaultAsync(x => x.Email.Equals(email));
            if (user.Image != string.Empty)
            {
                await _storageService.DeleteFileAsync(user.Image);
            }
            user.Image = await _image.SaveFile(image);
            _dataContext.User.Update(user);
            await _dataContext.SaveChangesAsync();
            return new ApiSuccessResult<string>("Cập nhập ảnh đại diện thành công");
        }
    }
}
