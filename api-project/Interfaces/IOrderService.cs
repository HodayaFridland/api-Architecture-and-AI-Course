using api_project.DTO;

namespace api_project.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderReadDto>?> AddItemToBasket(int userId, int giftId, int quantity = 1);
        Task<bool> ConfirmOrder(int userId);
        Task<IEnumerable<GiftReadDTO>> GetAllGifts(int? minprice, int? maxprice, string? category);
        Task<IEnumerable<OrderReadDto>> GetAllOrdersByGiftAsync(int giftId);
        Task<IEnumerable<GiftReadDTO>> GetGiftsDescAsync();
        Task<IEnumerable<UserDTOs.UserReadDTO>> GetPurchasersAsync();
        Task<IEnumerable<GiftReadDTO>> GetThePopularGiftAsync();
        Task<IEnumerable<GiftReadWithWinnerDTO>> GiftReadWithWinnerDTOs();
        Task<bool> RemoveItemFromBasket(int userId, int giftId);
        Task<IEnumerable<OrderDetailsDto>> GetBasketByUserId(int userId);
    }
}