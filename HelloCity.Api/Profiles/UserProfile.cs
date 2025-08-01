using AutoMapper;
using HelloCity.Models.Entities;
using HelloCity.Api.DTOs.Users;

namespace HelloCity.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Users, UserDto>();
            CreateMap<CreateUserDto, Users>();
            CreateMap<EditUserDto, Users>();
        }
    }
}
