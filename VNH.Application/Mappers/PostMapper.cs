using AutoMapper;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Domain;

namespace VNH.Application.Mappers
{
    public class PostMapper : Profile
    {
        public PostMapper()
        {
            CreateMap<CreatePostDto, Post>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<Post, PostResponseDto>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<ReportPostDto, PostReportDetail>().ReverseMap();

        }
    }
}
