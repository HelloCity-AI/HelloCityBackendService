using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloCity.Models.Entities;

namespace HelloCity.IServices
{
    public interface IUserService
    {
        Task<Users?> GetUserProfileAsync(Guid userId);
        Task<Users> CreateUserAsync(Users user);
        Task<Users> EditUserAsync(Guid id, Users user);
        Task<Users?> GetBySubIdAsync(string subId);
    }
}
