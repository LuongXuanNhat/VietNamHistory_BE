using VNH.Application.DTOs.Catalog.HashTags;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Domain;

namespace VNH.Application.Interfaces.Catalog.HashTags
{
    public interface IHashTag
    {
        Task<List<TagDto>> GetAll();
        Task AddTag(Tag tag);
        Task<ApiResult<List<string>>> GetAllTag(int numberTag);
    }
}
