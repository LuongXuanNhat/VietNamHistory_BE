using Microsoft.AspNetCore.Http;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common.ResponseNotification;

namespace VNH.Application.Services.Catalog.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> GetImage(string request);
        Task<ApiResult<UserDetailDto>> GetUserDetail(string email);
        Task<ApiResult<string>> SetImageUser(string name, IFormFile image);
        Task<ApiResult<UserDetailDto>> Update(UserUpdateDto request);



        // SendConfirmCodeToEmail
    }
}
