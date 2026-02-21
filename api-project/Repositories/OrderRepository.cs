using api_project.Data;
using api_project.Interfaces;
using api_project.Models;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;

namespace api_project.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Order>> GetAllOrdersByGiftAsync(int giftId)
        {
            return await _context.Orders
                .Where(o => o.GiftId == giftId && o.IsConfirmed).Include(o=>o.User)
                .ToListAsync();
        }
        public async Task<IEnumerable<Gift>> GetGiftsDesc()
        {
            return await _context.Gifts
           .OrderByDescending(g => g.Price)
           .ToListAsync();
        }
        public async Task<IEnumerable<Gift>> GetThePopularGiftAsync()
        {
            return await _context.Gifts
               .OrderByDescending(g =>
                _context.Orders.Count(o => o.GiftId == g.Id && o.IsConfirmed))
                .ToListAsync();
        }
        public async Task<IEnumerable<User>> GetPurchasers()
        {
            return await _context.Orders
                .Where(o => o.IsConfirmed)
                .Select(o => o.User!)
                .Distinct()
                .ToListAsync();
        }








        ////users fuctions
        public async Task<IEnumerable<Gift>> GetAllGifts(int? minprice, int? maxprice, string? category)
        {
            return await _context.Gifts.Where(g =>
                (!minprice.HasValue || g.Price >= minprice.Value) &&
                (!maxprice.HasValue || g.Price <= maxprice.Value) &&
                (string.IsNullOrEmpty(category) || g.CategoryName == category)
            ).ToListAsync();
        }
        public async Task<IEnumerable<Order?>?> AddItemToBasket(int userId, int giftId, int quantity = 1)
        {
            if (await IsHasWinner(giftId))
                return null;
            for (int i = 0; i < quantity; i++)
            {
                var order = new Order
                {
                    UserId = userId,
                    GiftId = giftId,
                    IsConfirmed = false
                };
                _context.Orders.Add(order);
            }
            await _context.SaveChangesAsync();

            var orders = await _context.Orders
                .Where(o => o.UserId == userId && !o.IsConfirmed)
                .Include(o => o.Gift)
                .ToListAsync();
            return orders;
        }

        public async Task<bool> ConfirmOrder(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId && !o.IsConfirmed)
                .Include(o => o.Gift)
                .Where(o => o.Gift != null && o.Gift.Winner == null)
                .ToListAsync();

            if (!orders.Any()) return false;
            foreach (var order in orders)
            {
                order.IsConfirmed = true;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsHasWinner(int giftId)
        {
            return await _context.Gifts.Where(g => g.Id == giftId)
                .AnyAsync(g => g.Winner != null);

        }
        public async Task<bool> RemoveItemFromBasket(int userId, int giftId)
        {
            Console.WriteLine(userId);
            Console.WriteLine(giftId);
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.UserId == userId && o.GiftId == giftId && !o.IsConfirmed);
            if (order == null) return false;
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<Gift>> GetAllGiftsWithWinners()
        {
            return await _context.Gifts.
                Include(g => g.Winner).Where(g=>g.Winner!=null)
               .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.Where(o => o.IsConfirmed)
            .ToListAsync();

        }
        public async Task<IEnumerable<Order>> GetAllOrdersWithGiftsAsync()
        {
            return await _context.Orders.Include(o=>o.Gift).Where(o => o.IsConfirmed)
            .ToListAsync();

        }
        public async Task<IEnumerable<Order>> GetBasketByUserId(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId && !o.IsConfirmed)
                .Include(o => o.Gift)
                .ToListAsync();
        }
    }
}
