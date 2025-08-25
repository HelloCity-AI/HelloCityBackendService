using HelloCity.Models.Entities;

namespace HelloCity.IRepository
{
    public interface IUserRepository
    {
        Task<Users?> GetUserByIdAsync(Guid userId);
        Task<Users> AddUserAsync(Users user);
        Task<Users> UpdateUserAsync(Users user);
        Task<Users?> GetUserBySubIdAsync(string subId);
    }
}
