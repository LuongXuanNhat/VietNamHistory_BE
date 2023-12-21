
namespace VNH.Application.Interfaces.Common
{
    public interface IStorageService
    {
        string GetFileUrl(string fileName);
        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);
        Task SaveImageFileAsync(Stream mediaBinaryStream, string fileName);
        Task DeleteFileAsync(string fileName);
        Task SaveDocFileAsync(Stream mediaBinaryStream, string fileName);
        Task DeleteDocFileAsync(string fileName);
    }
}
