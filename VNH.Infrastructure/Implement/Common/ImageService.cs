using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.Interfaces.Common;

namespace VNH.Infrastructure.Implement.Common
{
    public class ImageService : IImageService
    {
        public string ConvertByteArrayToString(byte[]? byteArray, Encoding encoding)
        {
            return encoding.GetString(byteArray) ?? string.Empty;
        }

        public async Task<byte[]> ConvertFormFileToByteArray(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
