using System.Drawing;
using api_project.Data;
using api_project.Models;
using Microsoft.EntityFrameworkCore;

namespace api_project.Repositories
{
    public class DonorRepository : IDonorRepository
    {
        private readonly ApplicationDbContext _context;

        public DonorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Donor?>> GetDonorsAsync(string? name,string? email,string? giftName)
        {
            return await _context.Donors
                .Include(d => d.Gifts)
                .Where(d =>
                    (string.IsNullOrEmpty(name) || d.DonorName.Contains(name)) &&
                    (string.IsNullOrEmpty(email) || d.Email.Contains(email)) &&
                    (string.IsNullOrEmpty(giftName) ||
                        d.Gifts.Any(g => g.GiftName.Contains(giftName)))
                )
                .ToListAsync();
        }

        public async Task<Donor?> GetDonorByIdAsync(int id)
        {
            return await _context.Donors
                .Include(d => d.Gifts)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Donor?> GetByEmailAsync(string email)
        {
            return await _context.Donors
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Donor> CreateDonorAsync(Donor donor)
        {
            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();
            return donor;
        }



        public async Task<Donor?> UpdateAsync(Donor donor)
        {
            var existing = await _context.Donors.FindAsync(donor.Id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(donor);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var donor = await _context.Donors.FindAsync(id);
            if (donor == null) return false;

            _context.Donors.Remove(donor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Donors.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Donors.AnyAsync(u => u.Email == email);
        }


    }
}
