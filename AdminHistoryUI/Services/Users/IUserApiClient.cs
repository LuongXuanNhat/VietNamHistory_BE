using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Domain;

namespace AdminHistoryUI.Services.Users
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<List<UserDetailDto>> getAllUser();

        Task<ApiResult<string>> RegisterUser(RegisterRequest registerRequest);

        Task<ApiResult<bool>> UpdateUser(Guid id,UserDetailDto request);
        Task<ApiResult<UserDetailDto>> GetById(Guid id);
        Task<ApiResult<bool>> Delete(Guid id);
    }
}
