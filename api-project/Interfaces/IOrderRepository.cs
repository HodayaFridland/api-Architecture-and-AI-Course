using api_project.Models;

namespace api_project.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order?>?> AddItemToBasket(int userId, int giftId, int quantity = 1);
        Task<bool> ConfirmOrder(int userId);
        Task<IEnumerable<Gift>> GetAllGifts(int? minprice, int? maxprice, string? category);
        Task<IEnumerable<Gift>> GetAllGiftsWithWinners();
        Task<IEnumerable<Order>> GetAllOrdersByGiftAsync(int giftId);
        Task<IEnumerable<Gift>> GetGiftsDesc();
        Task<IEnumerable<User>> GetPurchasers();
        Task<IEnumerable<Gift>> GetThePopularGiftAsync();
        Task<bool> IsHasWinner(int giftId);
        Task<bool> RemoveItemFromBasket(int userId, int giftId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetBasketByUserId(int userId);
        Task<IEnumerable<Order>> GetAllOrdersWithGiftsAsync();
    }
}