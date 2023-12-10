using VNH.Application.DTOs.Catalog.Document;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Common.ResponseNotification;

namespace VNH.Application.Interfaces.Documents
{
    public interface IDocumentService
    {
        Task<ApiResult<DocumentReponseDto>> Create(CreateDocumentDto requestDto, string name);
        Task<ApiResult<DocumentReponseDto>> Update(CreateDocumentDto requestDto, string name);
        Task<ApiResult<DocumentReponseDto>> Detail(string Id);
        Task<ApiResult<List<DocumentReponseDto>>> GetAll();
        Task<ApiResult<string>> Delete(string id, string email);

        Task<ApiResult<bool>> GetSave(DocumentFpkDto docsFpk);

        Task<ApiResult<int>> AddOrRemoveSaveDocs(DocumentFpkDto docsFpk);
        Task<ApiResult<List<DocumentReponseDto>>> Search(string keyWord);
    }
}
