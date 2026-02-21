using System.Drawing;
using api_project.Data;
using api_project.DTO;
using api_project.Interfaces;
using api_project.Models;
using Microsoft.EntityFrameworkCore;

namespace api_project.Repositories
{
    public class GiftRepository : IGiftRepository
    {
        private readonly ApplicationDbContext _context;
        public GiftRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Gift>> GetAllGiftsAsync()
        {
            return await _context.Gifts.ToListAsync();
        }
        public async Task<IEnumerable<Gift>> GetGiftsWhithOutWhiners()
        {
            return await _context.Gifts.Where(g=>g.WinnerId==null).ToListAsync();
        }
        public async Task<Gift?> GetGiftByIdAsync(int id)
        {
            return await _context.Gifts
                .FindAsync(id);
        }
        public async Task<Gift> CreateGift(Gift gift)
        {
            _context.Gifts.Add(gift);
            await _context.SaveChangesAsync();
            return gift;
        }
        public async Task<Gift?> UpdateAsync(Gift gift)
        {
            var existing = await _context.Gifts.FindAsync(gift.Id);
            if (existing == null) return null;
            _context.Entry(existing).CurrentValues.SetValues(gift);
            await _context.SaveChangesAsync();
            return existing;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var gift = await _context.Gifts.FindAsync(id);
            if (gift == null) return false;
            if (gift.WinnerId != null) return false;
            _context.Gifts.Remove(gift);
            await _context.SaveChangesAsync();
            return true;
        }
        //public async Task<Donor?> GetDonorByGiftId(int giftID)
        //{
        //    var donor = await _context.Gift
        //        .Where(g => g.Id == giftID)
        //        .Include(g => g.Donor)
        //        .Select(g => g.Donor)
        //        .FirstOrDefaultAsync();
        //    return donor;
        //}
        public async Task<Donor?> GetDonorByGiftIdAsync(int giftId)
        {
            var gift = await _context.Gifts
                .Include(g => g.Donor)
                .FirstOrDefaultAsync(g => g.Id == giftId);
            return gift?.Donor;
        }
        public async Task<Gift?> GetGiftByName(string giftName)
        {
            return await _context.Gifts
                .FirstOrDefaultAsync(g => g.GiftName == giftName);
        }
        public async Task<IEnumerable<Gift>> GetGiftByDonorName(string donorName)
        {
            return await _context.Gifts
                .Include(g => g.Donor)
                .Where(g => g.Donor != null && g.Donor.DonorName == donorName)
                .ToListAsync();
        }
        public async Task<IEnumerable<Gift?>> GetGiftByNumOfPurchases(int numOfPurchases)
        {
            return await _context.Gifts
                 .Where(g => g.NumOfPurchases == numOfPurchases)
                 .ToListAsync();
        }
        public async Task<decimal> GetPriceByGiftName(int giftId)
        {
            var gift = await _context.Gifts
                .FirstOrDefaultAsync(g => g.Id == giftId);
            return gift != null ? gift.Price : 0;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
