using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Application.Interfaces.Common
{
    public interface IImageService
    {
        Task<string> SaveFile(IFormFile file);
        Task<string> SaveImageArticle(IFormFile file);
        Task<byte[]> ConvertFormFileToByteArray(IFormFile formFile);
        string ConvertByteArrayToString(byte[]? byteArray, Encoding encoding);
        byte[] CompressImage(byte[] originalImage, int KbNumber);
    }
}
