using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Domain;

namespace VNH.Application.Interfaces.Catalog.Accounts
{
    public interface IAccountService
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<string>> Register(RegisterRequest request);
        Task<ApiResult<string>> EmailConfirm(string numberConfirm, string email);
        ClaimsPrincipal ValidateToken(string jwtToken);
        Task<ApiResult<LoginRequest>> ForgetPassword(string email);
        Task<ApiResult<ResetPassDto>> ConfirmCode(string email);
        Task<ApiResult<string>> ResetPassword(ResetPassDto resetPass);
        Task LockAccount(User user);
        Task<ApiResult<string>> ChangePassword(ChangePasswordDto changePasswodDto);
        Task<ApiResult<string>> LoginExtend(string email, string name);
        Task<ApiResult<string>> ChangeEmail(string currentEmail ,string email);
    }
}
