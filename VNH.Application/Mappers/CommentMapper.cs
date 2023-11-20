using AutoMapper;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Domain;

namespace VNH.Application.Mappers
{
    public class CommentMapper : Profile
    {
        public CommentMapper() {
            CreateMap<PostComment, CommentPostDto>().ReverseMap();
            CreateMap<PostSubComment, SubCommentDto>().ReverseMap();
        }

    }
}
