using AutoMapper;
using UserService.DTOs.User;
using WebApplication4.DTOs.User;
using WebApplication4.Models;

namespace UserService.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<User, UserDto>();
        }
    }
}
