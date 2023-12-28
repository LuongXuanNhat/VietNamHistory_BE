using Microsoft.AspNetCore.Hosting;
using VNH.Application.Interfaces.Common;

namespace VNH.Infrastructure.Implement.Common
{
    public class StorageService : IStorageService
    {
        private readonly string _userContentFolder;
        private readonly string _userImageFolder;
        private readonly string _userDocFolder;
        private const string USER_CONTENT_FOLDER_NAME = "Images";
        private const string USER_DOC_FOLDER_NAME = "Documents";
        private const string USER_IMAGE_FOLDER_NAME = "ArticleImages";

        public StorageService(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolder = Path.Combine(webHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME);
            _userDocFolder = Path.Combine(webHostEnvironment.WebRootPath, USER_DOC_FOLDER_NAME);
            _userImageFolder = Path.Combine(webHostEnvironment.WebRootPath, USER_IMAGE_FOLDER_NAME);
        }

        public string GetFileUrl(string fileName)
        {
            return $"/{USER_CONTENT_FOLDER_NAME}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }
        public async Task SaveImageFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userImageFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }
        public async Task SaveDocFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userDocFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }
        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
        public async Task DeleteDocFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userDocFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
