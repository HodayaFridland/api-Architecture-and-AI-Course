using api_project.Models;

namespace api_project.Repositories
{
    public interface IDonorRepository
    {
        Task<Donor> CreateDonorAsync(Donor donor);
        Task<bool> DeleteAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<IEnumerable<Donor?>> GetDonorsAsync(string? name, string? email, string? giftName);
        Task<Donor?> GetByEmailAsync(string email);
        Task<Donor?> GetDonorByIdAsync(int id);
        Task<Donor?> UpdateAsync(Donor donor);
    }
}