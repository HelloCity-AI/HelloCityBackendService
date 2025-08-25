using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloCity.IRepository;
using HelloCity.Models;
using HelloCity.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelloCity.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get user by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Users?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        /// <summary>
        /// Get user by SubId
        /// </summary>
        /// <param name="subId"></param>
        /// <returns></returns>
        public async Task<Users?> GetUserBySubIdAsync(string subId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.SubId == subId);
        }

        /// <summary>
        /// Post user profile
        /// </summary>
        ///to be added
        /// <returns></returns>
        public async Task<Users> AddUserAsync(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Users> UpdateUserAsync(Users user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
