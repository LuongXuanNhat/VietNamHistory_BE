using AutoMapper;
using VNH.Application.DTOs.Catalog.HashTags;
using VNH.Domain;

namespace VNH.Application.Mappers
{
    public class TagMapper : Profile
    {
        public TagMapper() {
            CreateMap<Tag, TagDto>();
        }
    }
}
