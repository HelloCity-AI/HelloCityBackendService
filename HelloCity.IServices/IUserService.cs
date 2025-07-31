using HelloCity.Models.Entities;

namespace HelloCity.IServices
{
    public interface IUserService
    {
        Task<Users?> GetUserProfileAsync(Guid userId);
        Task<Users> CreateUserAsync(Users user);
    }
}
