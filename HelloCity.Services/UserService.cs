using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HelloCity.IServices;
using HelloCity.IRepository;
using HelloCity.Models.DTOs.Users;
using HelloCity.Models.Entities;

namespace HelloCity.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get user profile by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserDto?> GetUserProfileAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Post user profile
        /// </summary>
        ///to be added
        /// <returns></returns>
        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            var user = _mapper.Map<Users>(dto);
            user.UserId = Guid.NewGuid();

            await _userRepository.AddUserAsync(user);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> EditUserAsync(Guid id, CreateUserDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            // var user = _mapper.Map<Users>(dto);
            _mapper.Map(dto, user);

            await _userRepository.UpdateUserAsync(user);

            return _mapper.Map<UserDto>(user);
        }

    }
}
