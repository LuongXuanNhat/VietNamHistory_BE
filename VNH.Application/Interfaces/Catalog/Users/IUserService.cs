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
        Task<ApiResult<UserDetailDto>> GetUserDetail(string email);
        Task<ApiResult<UserDetailDto>> Update(UserUpdateDto request);



        // SendConfirmCodeToEmail
    }
}
