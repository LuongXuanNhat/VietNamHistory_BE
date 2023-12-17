using Microsoft.AspNetCore.Http;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.DTOs.Common.Users;

namespace VNH.Application.Services.Catalog.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> GetImage(string request);
        Task<ApiResult<UserDetailDto>> GetUserDetail(string email);
        Task<ApiResult<string>> SetImageUser(string name, IFormFile image);
        Task<ApiResult<UserDetailDto>> Update(UserUpdateDto request);
        Task<ApiResult<UserDetailDto>> GetUserById(string id);
        Task<ApiResult<bool>> UpdateForAdmin(Guid id, UserUpdateDto request);
        Task<ApiResult<bool>> Delete(Guid id);
        Task<ApiResult<List<UserDetailDto>>> getAllUser();

        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);



      
    }
}
