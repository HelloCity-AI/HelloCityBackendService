using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloCity.Api.DTOs.Users;

namespace HelloCity.IServices
{
    public interface IUserService
    {
        Task<UserDto?> GetUserProfileAsync(Guid userId);
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
    }
}
