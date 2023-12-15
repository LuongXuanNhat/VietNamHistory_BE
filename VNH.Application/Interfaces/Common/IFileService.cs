using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Application.Interfaces.Common
{
    public interface IFileService
    {
        Task<string> SaveFile(IFormFile file, string title);

    }
}
