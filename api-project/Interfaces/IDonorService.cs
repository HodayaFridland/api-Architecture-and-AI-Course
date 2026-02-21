using api_project.DTO;

namespace api_project.Interfaces
{
    public interface IDonorService
    {
        Task<DonorReadDTO> CreateDonorAsync(DonorCreateDTO DonorCreateDTO);
        Task<bool> DeleteDonorAsync(int id);
        Task<List<DonorReadDTO>> GetAllDonorsAsync(string? name,string? email,string? giftName);
        Task<DonorGiftsReadDTO?> GetDonorByIdAsync(int id);
        Task<DonorReadDTO?> UpdateDonorAsync(int id, DonorUpdateDTO updateDonor);
    }
}