
using ImageMagick;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;
using VNH.Application.Common.Contants;
using VNH.Application.Interfaces.Common;

namespace VNH.Infrastructure.Implement.Common
{
    public class FileService : IFileService
    {
        private const string USER_CONTENT_FOLDER_NAME = "Documents";
        private readonly
             string URL = SystemConstants.UrlWeb;
        private readonly IStorageService _storageService;
        public FileService(IStorageService fileService)
        {
            _storageService = fileService;
        }

        public async Task<string> SaveFile(IFormFile file, string title)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName?.Trim('"');
            var fileName = $"{title}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveDocFileAsync(file.OpenReadStream(), fileName);
            return URL + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

    }
}
