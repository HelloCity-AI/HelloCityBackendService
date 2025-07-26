using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using HelloCity.Models.Entities;


namespace HelloCity.IRepository
{
    public interface IUserRepository
    {
        Task<Users?> GetUserByIdAsync(Guid userId);
        Task<Users> AddUserAsync(Users user);
    }
}
