using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloCity.IServices;
using HelloCity.IRepository;
using HelloCity.Models.Entities;
using HelloCity.Models.Utils;

namespace HelloCity.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Get user profile by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Users?> GetUserProfileAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        /// <summary>
        /// Post user profile
        /// </summary>
        ///to be added
        /// <returns></returns>

        public async Task<Users> CreateUserAsync(Users user)
        {
            user.UserId = Guid.NewGuid();
            user.LastJoinDate = DateTimeHelper.GetSydneyNow();

            await _userRepository.AddUserAsync(user);

            return user;
        }

        public async Task<Users> EditUserAsync(Guid id, Users updatedUser)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            existingUser.Username = updatedUser.Username;
            existingUser.City = updatedUser.City;
            existingUser.Nationality = updatedUser.Nationality;
            existingUser.PreferredLanguage = updatedUser.PreferredLanguage;
            existingUser.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateUserAsync(existingUser);

            return existingUser;
        }

        public async Task<UserDto> EditUserAsync(Guid id, UserInfoCollectionDTO dto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            // var user = _mapper.Map<Users>(dto);
            _mapper.Map(dto, user);

            await _userRepository.UpdateUserAsync(user);

            return _mapper.Map<UserDto>(user);
        }

    }
}
