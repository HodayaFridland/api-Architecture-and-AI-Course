using api_project.DTO;
using api_project.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static api_project.DTO.UserDTOs;

namespace api_project.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class LotteryController : ControllerBase
    {
        private readonly ILotteryService _lotteryService;
        public LotteryController(ILotteryService lotteryService)
        {
            _lotteryService = lotteryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GiftReadDTO>>> GiftsWithoutWinners()
        {
            var gifts=await _lotteryService.GiftsWithoutWinners();
            return Ok(gifts);
        }

        [HttpPost("gift/{giftId}")]
        public async Task<ActionResult<UserReadDTO?>> LotteryForGiftAsync(int giftId)
        {
            var winner = await _lotteryService.LotteryForGiftAsync(giftId);
            if (winner == null)
            {
                return NotFound("No winner could be determined for the specified gift.");
            }
            return Ok(winner);
        }
        [HttpGet("income-total")]
        public async Task<ActionResult<decimal>> GetTotalIncome()
        {
            var totalIncome = await _lotteryService.GetTotalIncome();
            return Ok(totalIncome);
        }
    }
}
