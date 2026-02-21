using api_project.DTO;
using static api_project.DTO.UserDTOs;

namespace api_project.Interfaces
{
    public interface ILotteryService
    {
        Task<decimal> GetTotalIncome();
        Task<UserReadDTO?> LotteryForGiftAsync(int giftId);
        Task<IEnumerable<GiftReadDTO>> GiftsWithoutWinners();
        //Task SendWinnerEmailAsync(string toEmail, string userName, string giftName); 
        }
}