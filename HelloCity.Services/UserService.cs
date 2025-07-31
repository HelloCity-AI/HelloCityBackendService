using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloCity.IServices;
using HelloCity.IRepository;
using HelloCity.Models.Entities;

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

            await _userRepository.AddUserAsync(user);

            return user;
        }

    }
}
