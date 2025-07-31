using AutoMapper;
using HelloCity.Models.Entities;
using HelloCity.Models.DTOs.Users;

namespace HelloCity.Models.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Users, UserDto>();
            CreateMap<CreateUserDto, Users>();
        }
    }
}
