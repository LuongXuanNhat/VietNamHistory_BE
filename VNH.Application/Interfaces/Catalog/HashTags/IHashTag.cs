using VNH.Application.DTOs.Catalog.HashTags;
using VNH.Domain;

namespace VNH.Application.Interfaces.Catalog.HashTags
{
    public interface IHashTag
    {
        Task<List<TagDto>> GetAll();
        Task AddTag(Tag tag);
    }
}
