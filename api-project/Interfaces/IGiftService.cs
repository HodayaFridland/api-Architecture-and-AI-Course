using api_project.DTO;

namespace api_project.Interfaces
{
    public interface IGiftService
    {
        Task<GiftReadDTO> CreateGiftAsync(GiftCreateDTO giftCreateDTO);
        Task<bool> DeleteGiftAsync(int id);
        Task<IEnumerable<GiftReadDTO?>> GetAllGiftsAsync();
        Task<DonorReadDTO?> GetDonorByGiftIdAsync(int giftId);
        Task<IEnumerable<GiftReadDTO?>> GetGiftByDonorNameAsync(string donorName);
        Task<GiftReadDTO?> GetGiftByIdAsync(int id);
        Task<GiftReadDTO?> GetGiftByNameAsync(string giftName);
        Task<IEnumerable<GiftReadDTO?>> GetGiftByNumOfPurchasesAsync(int numOfPurchases);
        Task<decimal> GetPriceByGiftIdAsync(int giftId);
        Task<GiftReadDTO?> UpdateGiftAsync(int id, GiftUpdateDto giftUpdateDTO);
    }
}