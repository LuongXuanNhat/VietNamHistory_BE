using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Drawing.Imaging;
using System.Net.Http.Headers;
using System.Text;
using VNH.Application.Common.Contants;
using VNH.Application.Interfaces.Common;

namespace VNH.Infrastructure.Implement.Common
{
    public class ImageService : IImageService
    {
        private const string USER_CONTENT_FOLDER_NAME = "Images";
        private const string USER_IMAGE_FOLDER_NAME = "ArticleImages";
        private readonly string URL = SystemConstants.UrlWeb;
        private readonly IStorageService _storageService;
        public ImageService(IStorageService storageService)
        {
            _storageService = storageService;
        }
        public async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName?.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return URL + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }
        public async Task<string> SaveImageArticle(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName?.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveImageFileAsync(file.OpenReadStream(), fileName);
            return URL + USER_IMAGE_FOLDER_NAME + "/" + fileName;
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
            if(originalImage == null || originalImage.Length <= targetSizeInKb * 1024)
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

        ////private static ImageCodecInfo GetEncoder(ImageFormat format)
        ////{
        ////    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
        ////    foreach (ImageCodecInfo codec in codecs)
        ////    {
        ////        if (codec.FormatID == format.Guid)
        ////        {
        ////            return codec;
        ////        }
        ////    }
        ////    throw new InvalidOperationException("Không thể nén ảnh");
        ////}
    }
    
}
