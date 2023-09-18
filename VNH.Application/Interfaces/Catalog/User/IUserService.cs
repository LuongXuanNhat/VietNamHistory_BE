using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common;

namespace VNH.Application.Services.Catalog.User
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
    }
}
