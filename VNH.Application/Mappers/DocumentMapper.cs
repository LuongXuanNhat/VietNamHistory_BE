using AutoMapper;
using VNH.Application.DTOs.Catalog.Document;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Domain;

namespace VNH.Application.Mappers
{
    public class DocumentMapper : Profile
    {
       public DocumentMapper()
        {
            CreateMap<CreateDocumentDto, Document>()
               .ForMember(dest => dest.FileName, opt => opt.Ignore());
            CreateMap<Document,DocumentReponseDto>().ForMember(dest => dest.FileName, opt => opt.Ignore());
        }
    }
}
