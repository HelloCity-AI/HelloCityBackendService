using AutoMapper;
using HelloCity.Models.DTOs.Users;
using HelloCity.Models.Entities;

namespace HelloCity.Tests.Helpers
{
    public class UserTestProfile : Profile
    {
        public UserTestProfile()
        {
            CreateMap<Users, UserDto>();
            CreateMap<CreateUserDto, Users>();
        }
    }
}
