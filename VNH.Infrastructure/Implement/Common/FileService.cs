
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

        public async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName?.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return URL + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public string ConvertByteArrayToString(byte[]? byteArray, Encoding encoding)
        {
            return byteArray is not null ? Convert.ToBase64String(byteArray) : string.Empty;
        }

        public async Task<byte[]> ConvertFormFileToByteArray(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public byte[] CompressImage(byte[] originalImage, int targetSizeInKb)
        {
            if (originalImage == null || originalImage.Length <= targetSizeInKb * 1024)
            {
                return originalImage;
            }

            using MagickImage image = new MagickImage(originalImage);
            int quality = 40; // Chất lượng ban đầu
            byte[] compressedData;

            while (true)
            {
                using (MagickImage clonedImage = new MagickImage(image))
                {
                    clonedImage.Quality = quality;
                    compressedData = clonedImage.ToByteArray(MagickFormat.Jpeg);
                }

                if (compressedData.Length <= targetSizeInKb * 1024)
                {
                    return compressedData;
                }
                quality -= 5;
                if (quality <= 2)
                {
                    return compressedData;
                }
            }
        }

    }
}
