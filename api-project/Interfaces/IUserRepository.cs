using api_project.Models;

namespace api_project.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetByEmailAsync(string email);
    }
}