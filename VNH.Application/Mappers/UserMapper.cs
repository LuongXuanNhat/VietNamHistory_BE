using AutoMapper;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Domain;

namespace VNH.Application.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper() {
            CreateMap<UserUpdateDto, User>()
                 .ForMember(dest => dest.Image, opt => opt.Ignore());
            CreateMap<User, UserDetailDto>();

        }
    }
}
