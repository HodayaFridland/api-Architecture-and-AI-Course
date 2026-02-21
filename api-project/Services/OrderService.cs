using api_project.DTO;
using api_project.Interfaces;
using api_project.Models;
using static api_project.DTO.UserDTOs;

namespace api_project.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<IEnumerable<OrderReadDto>> GetAllOrdersByGiftAsync(int giftId)
        {
            var orders = await _orderRepository.GetAllOrdersByGiftAsync(giftId);
            return orders.Select(o => new OrderReadDto
            {
                Id = o.Id,
                UserId = o.UserId,
                GiftId = o.GiftId,
                IsConfirmed = o.IsConfirmed,
                User=o.User
            }).ToList();

        }
        public async Task<IEnumerable<GiftReadDTO>> GetGiftsDescAsync()
        {
            var gifts = await _orderRepository.GetGiftsDesc();
            return gifts.Select(g => new GiftReadDTO
            {
                Id = g.Id,
                GiftName = g.GiftName,
                Price = g.Price
            }).ToList();

        }
        public async Task<IEnumerable<GiftReadDTO>> GetThePopularGiftAsync()
        {
            var gifts = await _orderRepository.GetThePopularGiftAsync();
            return gifts.Select(g => new GiftReadDTO
            {
                Id = g.Id,
                GiftName = g.GiftName,
                Price = g.Price
            }).ToList();
        }
        public async Task<IEnumerable<UserReadDTO>> GetPurchasersAsync()
        {
            var users = await _orderRepository.GetPurchasers();
            return users.Select(u => new UserReadDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Phone = u.Phone
            }).ToList();
        }
        public async Task<IEnumerable<GiftReadDTO>> GetAllGifts(int? minprice, int? maxprice, string? category)
        {
            var gifts = await _orderRepository.GetAllGifts(minprice, maxprice, category);
            return gifts.Select(g => new GiftReadDTO
            {
                Id = g.Id,
                GiftName = g.GiftName,
                Price = g.Price,
                ImageUrl=g.ImageUrl,
                CategoryName=g.CategoryName,
                DonorId=g.DonorId
            }).ToList();
        }
        public async Task<IEnumerable<OrderReadDto>?> AddItemToBasket(int userId, int giftId, int quantity = 1)
        {
            var orders = await _orderRepository.AddItemToBasket(userId, giftId, quantity);
            if (orders == null)
                return null;
            return orders.Select(o => new OrderReadDto
            {
                Id = o!.Id,
                UserId = o.UserId,
                GiftId = o.GiftId,
                IsConfirmed = o.IsConfirmed
            }).ToList();
        }
        public async Task<bool> ConfirmOrder(int userId)
        {
            return await _orderRepository.ConfirmOrder(userId);
        }

        public async Task<bool> RemoveItemFromBasket(int userId, int giftId)
        {
            return await _orderRepository.RemoveItemFromBasket(userId, giftId);
        }



        public async Task<IEnumerable<GiftReadWithWinnerDTO>> GiftReadWithWinnerDTOs()
        {
            var gifts = await _orderRepository.GetAllGiftsWithWinners();

            return gifts
                .Where(g => g.Winner != null)
                .Select(g => new GiftReadWithWinnerDTO
                {
                    Id = g.Id,
                    GiftName = g.GiftName,
                    Price = g.Price,
                    Winner = new UserReadDTO
                    {
                        Id = g.Winner!.Id,
                        Name = g.Winner.Name,
                        Email = g.Winner.Email,
                        Phone = g.Winner.Phone
                    }
                }).ToList();
        }
        public async Task<IEnumerable<OrderDetailsDto>> GetBasketByUserId(int userId)
        {
            var orders = await _orderRepository.GetBasketByUserId(userId);
            return orders.Select(o => new OrderDetailsDto
            {
                Id = o.Id,
                UserId = o.UserId,
                GiftId = o.GiftId,
                IsConfirmed = o.IsConfirmed,
                Gift = new GiftReadDTO
                {
                    Id = o.Gift!.Id,
                    GiftName = o.Gift.GiftName,
                    Price = o.Gift.Price,
                    ImageUrl = o.Gift.ImageUrl
                }
            }).ToList();

        }
    }
}