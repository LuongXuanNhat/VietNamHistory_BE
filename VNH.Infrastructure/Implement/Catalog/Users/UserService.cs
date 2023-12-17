using Microsoft.AspNetCore.Identity;
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
using VNH.Application.DTOs.Common.Users;
using Microsoft.AspNetCore.Authorization.Infrastructure;

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

        public async Task<ApiResult<UserDetailDto>> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
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


        public async Task<ApiResult<bool>> UpdateForAdmin(Guid id,UserUpdateDto request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id))
            {
                return new ApiErrorResult<bool>("Emai đã tồn tại");
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            _mapper.Map(request, user);
            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            else
            {
                return new ApiErrorResult<bool>("Cập nhật không thành công");

            }

        }




        public async Task<ApiResult<string>> GetImage(string email)
        {
            var user = await _dataContext.User.FirstOrDefaultAsync(x => x.Email.Equals(email));
            if (user is null) return new ApiSuccessResult<string>(string.Empty);
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

        public async Task<ApiResult<List<UserDetailDto>>> getAllUser()
        {
            var result = await _dataContext.Users.ToListAsync();
            var response = _mapper.Map<List<UserDetailDto>>(result);
            return new ApiSuccessResult<List<UserDetailDto>>(response);
        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("User không tồn tại");
            }
            user.IsDeleted = true;
            var reult = await _userManager.UpdateAsync(user);

            if (reult.Succeeded)
                return new ApiSuccessResult<bool>();

            return new ApiErrorResult<bool>("Xóa không thành công");
        }

        public async Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản không tồn tại");
            }
            var removedRoles = request.Roles.Where(x => x.Selected == false).Select(x => x.Name).ToList();
            foreach (var roleName in removedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == true)
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName);
                }
            }
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            var addedRoles = request.Roles.Where(x => x.Selected).Select(x => x.Name).ToList();
            foreach (var roleName in addedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == false)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            return new ApiSuccessResult<bool>();
        }
    }
}
