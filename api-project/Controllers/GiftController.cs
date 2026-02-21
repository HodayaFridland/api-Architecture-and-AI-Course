using api_project.DTO;
using api_project.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Diagnostics;
using System.Text.Json;

namespace api_project.Controllers
{
    
    [Authorize]
    [ApiController]
        [Route("api/[controller]")]
        public class GiftController : ControllerBase
        {
            private const string AllGiftsCacheKey = "all-gifts";

            private readonly IGiftService _giftService;
            private readonly ILogger<GiftController> _logger;
            private readonly IDistributedCache _cache;

            public GiftController(IGiftService giftService, ILogger<GiftController> logger, IDistributedCache cache)
            {
                _giftService = giftService;
                _logger = logger;
                _cache = cache;
            }
         
            [HttpGet]
            public async Task<ActionResult<List<GiftReadDTO>>> GetAllGifts()
            {
                var sw = Stopwatch.StartNew();

                // Cache HIT — return directly from Redis
                var cached = await _cache.GetStringAsync(AllGiftsCacheKey);
                if (cached is not null)
                {
                    sw.Stop();
                    _logger.LogInformation("Cache HIT for [{Key}] — served in {Ms}ms", AllGiftsCacheKey, sw.ElapsedMilliseconds);
                    return Ok(JsonSerializer.Deserialize<List<GiftReadDTO>>(cached));
                }

                // Cache MISS — query DB, then store in Redis (TTL: 30s for testing, increase to 300s in production)
                var gifts = await _giftService.GetAllGiftsAsync();

                await _cache.SetStringAsync(AllGiftsCacheKey, JsonSerializer.Serialize(gifts),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300)
                    });

                sw.Stop();
                _logger.LogInformation("Cache MISS for [{Key}] — queried DB and cached, served in {Ms}ms", AllGiftsCacheKey, sw.ElapsedMilliseconds);
                return Ok(gifts);
            }


        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
            public async Task<ActionResult<GiftReadDTO>> GetGiftById(int id)
            {
                var gift = await _giftService.GetGiftByIdAsync(id);
                if (gift == null)
                {
                    _logger.LogWarning("Gift with id {Id} not found", id);
                    return NotFound();
                }
                return Ok(gift);

            }
        [Authorize(Roles = "Admin")]
        [HttpPost]
            public async Task<ActionResult<GiftReadDTO>> CreateGift(GiftCreateDTO createDTO)
            {
                var gift = await _giftService.CreateGiftAsync(createDTO);
                await _cache.RemoveAsync(AllGiftsCacheKey);
                return CreatedAtAction(nameof(GetGiftById), new { id = gift.Id }, gift);
            }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
            public async Task<ActionResult<GiftReadDTO>> UpdateGift(int id, GiftUpdateDto updateDTO)
            {
                var gift = await _giftService.UpdateGiftAsync(id, updateDTO);
                if (gift == null)
                {
                    _logger.LogWarning("Gift with id {Id} not found for update", id);
                    return NotFound();
                }
                await _cache.RemoveAsync(AllGiftsCacheKey);
                return Ok(gift);
            }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
            public async Task<ActionResult> DeleteGift(int id)
            {
                var success = await _giftService.DeleteGiftAsync(id);
                if (!success)
                {
                    _logger.LogWarning("Gift with id {Id} not found for deletion", id);
                    return NotFound();
                }
                await _cache.RemoveAsync(AllGiftsCacheKey);
                return NoContent();
            }
        [Authorize(Roles = "Admin")]
        [HttpGet("donor/{giftId}")]
            public async Task<ActionResult<DonorReadDTO>> GetDonorByGiftId(int giftId)
            {
                var donor = await _giftService.GetDonorByGiftIdAsync(giftId);
                if (donor == null)
                {
                    _logger.LogWarning("Donor for gift id {GiftId} not found", giftId);
                    return NotFound();
                }
                return Ok(donor);
            }
      
        [Authorize(Roles = "Admin")]
        [HttpGet("donor/by-name/{donorName}")]
        public async Task<ActionResult<IEnumerable<GiftReadDTO>>> GetGiftByDonorName(string donorName)
            {
            try
            {
                var gifts = await _giftService.GetGiftByDonorNameAsync(donorName);
                if (gifts == null || !gifts.Any())
                    return NotFound($"No gifts found for donor {donorName}");

                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching gifts for donor {DonorName}", donorName);
                return StatusCode(500, "Internal server error");
            }
          
            }

        [HttpGet("by-name/{name}")]
            public async Task<ActionResult<GiftReadDTO>> GetGiftByName(string name)
            {
                var gift = await _giftService.GetGiftByNameAsync(name);
                if (gift == null)
                {
                    _logger.LogWarning("Gift with name {Name} not found", name);
                    return NotFound();
                }
                return Ok(gift);
            }

        [HttpGet("price/{giftId}")]
            public async Task<ActionResult<decimal>> GetPriceByGiftId(int giftId)
            {
                var price = await _giftService.GetPriceByGiftIdAsync(giftId);
                return Ok(price);
            }
        [Authorize(Roles = "Admin")]
        [HttpGet("bypurchase/{numOfPurchase}")]
            public async Task<ActionResult<List<GiftReadDTO>>> GetGiftByNumOfPurchase(int numOfPurchase)
            {
                var gifts = await _giftService.GetGiftByNumOfPurchasesAsync(numOfPurchase);
                return Ok(gifts);
            }

        }
}
