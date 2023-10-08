using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Domain;

namespace VNH.Application.Mappers
{
    public class PostMapper : Profile
    {
        public PostMapper()
        {
            CreateMap<CreatePostDto, Post>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<Post, PostResponsetDto>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

        }
    }
}
