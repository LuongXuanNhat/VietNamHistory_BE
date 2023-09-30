using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Domain;

namespace VNH.Application.Services.Catalog.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<bool>> Register(RegisterRequest request);
        Task<ApiResult<bool>> EmailConfirm(string numberConfirm, string email);
        ClaimsPrincipal ValidateToken(string jwtToken);
        Task<ApiResult<LoginRequest>> ForgetPassword(string email);
        Task<ApiResult<ResetPassDto>> ConfirmCode(LoginRequest loginRequest);
        Task<ApiResult<bool>> ResetPassword(ResetPassDto resetPass);
        Task LockAccount(User user);
        Task<ApiResult<UserDetailDto>> GetUserDetail(string email);
        Task<ApiResult<UserDetailDto>> Update(UserUpdateDto request);



        // SendConfirmCodeToEmail
    }
}
