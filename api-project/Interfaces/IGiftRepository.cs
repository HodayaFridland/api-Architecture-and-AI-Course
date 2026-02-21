using api_project.DTO;
using api_project.Models;

namespace api_project.Interfaces
{
    public interface IGiftRepository
    {
        Task<Gift> CreateGift(Gift gift);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Gift>> GetAllGiftsAsync();
        Task<Donor?> GetDonorByGiftIdAsync(int giftId);
        Task<IEnumerable<Gift>> GetGiftByDonorName(string donorName);
       Task<IEnumerable<Gift>> GetGiftsWhithOutWhiners();
        Task<Gift?> GetGiftByIdAsync(int id);
        Task<Gift?> GetGiftByName(string giftName);
        Task<IEnumerable<Gift?>> GetGiftByNumOfPurchases(int numOfPurchases);
        Task<decimal> GetPriceByGiftName(int giftId);
        Task<Gift?> UpdateAsync(Gift gift);
        Task SaveChangesAsync();
      
    }
}