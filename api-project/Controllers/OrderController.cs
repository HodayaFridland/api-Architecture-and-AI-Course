using System.Security.Claims;
using api_project.DTO;
using api_project.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static api_project.DTO.UserDTOs;

namespace api_project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("gift/{giftId}")]
        public async Task<ActionResult<IEnumerable<OrderReadDto>>> GetAllOrdersByGiftId(int giftId)
        {
            var orders = await _orderService.GetAllOrdersByGiftAsync(giftId);
            return Ok(orders);
        }


        [HttpGet("gifts/desc")]
        public async Task<ActionResult<IEnumerable<GiftReadDTO>>> GetGiftsDesc()
        {
            var gifts = await _orderService.GetGiftsDescAsync();
            return Ok(gifts);
        }

        [HttpGet("gifts/popular")]
        public async Task<ActionResult<IEnumerable<GiftReadDTO>>> GetThePopularGifts()
        {
            _logger.LogInformation("Fetching the most popular gifts.");
            var gifts = await _orderService.GetThePopularGiftAsync();
            return Ok(gifts);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("purchasers")]
        public async Task<ActionResult<IEnumerable<UserReadDTO>>> GetPurchasers()
        {
            _logger.LogInformation("Fetching list of purchasers.");
            var users = await _orderService.GetPurchasersAsync();
            return Ok(users);
        }


        [HttpGet("gifts")]
        public async Task<ActionResult<IEnumerable<GiftReadDTO>>> GetAllGifts([FromQuery] int? minprice, [FromQuery] int? maxprice, [FromQuery] string? category)
        {
            var gifts = await _orderService.GetAllGifts(minprice, maxprice, category);
            return Ok(gifts);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("gifts/winners")]
        public async Task<ActionResult<IEnumerable<GiftReadWithWinnerDTO>>> GetGiftsWithWinners()
        {
            var giftsWithWinners = await _orderService.GiftReadWithWinnerDTOs();
            return Ok(giftsWithWinners);
        }

        [HttpPost("basket/add")]
        public async Task<ActionResult<IEnumerable<OrderReadDto>?>> AddItemToBasket([FromQuery]int giftId, [FromQuery] int quantity )
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var orders = await _orderService.AddItemToBasket(userId, giftId, quantity);
            if (orders == null)
            {
                _logger.LogWarning("Attempt to add item to basket failed: Gift {GiftId} already has a winner.", giftId);
                return BadRequest("Cannot add item to basket. The gift already has a winner.");
            }
            return Ok(orders);
        }

        [HttpPut("basket/confirm")]
        public async Task<ActionResult<bool>> ConfirmOrder()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _orderService.ConfirmOrder(userId);
            if (result == false)
            {
                _logger.LogWarning("Attempt to confirm order failed: No items in basket for User {UserId}.", userId);
                return Conflict("Cannot confirm order. No items in basket.");
            }
            return Ok(result);

        }
     

        [HttpDelete("basket/remove/{giftId}")]
        public async Task<ActionResult<bool>> RemoveItemFromBasket([FromRoute] int giftId)
        {

            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _orderService.RemoveItemFromBasket(userId, giftId);
            if (result == false)
            {
                _logger.LogWarning("Attempt to remove item from basket failed: Item not found for User {UserId} and Gift {GiftId}.", userId, giftId);
                return NotFound("Item not found in basket.");
            }
            return Ok(result);
        }

       

        [HttpGet("basket")]
        public async Task<ActionResult<IEnumerable<OrderReadDto>>> GetBasket()
        {
            var userId = int.Parse(
               User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var orders = await _orderService.GetBasketByUserId(userId);
            return Ok(orders);

        }
    }
}