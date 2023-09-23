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
        Task<ApiResult<ResetPassDTO>> ConfirmCode(LoginRequest loginRequest);
        Task<ApiResult<bool>> ResetPassword(ResetPassDTO resetPass);
        Task LockAccount(User user);



        // SendConfirmCodeToEmail
    }
}
