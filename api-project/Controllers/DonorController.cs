using api_project.DTO;
using api_project.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_project.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donorService;

        public DonorController(IDonorService donorService)
        {
            _donorService = donorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonorReadDTO>>> GetAllDonorsAsync([FromQuery] string? name, [FromQuery] string? email, [FromQuery] string? giftName)
        {
            var donors = await _donorService.GetAllDonorsAsync(name,email,giftName);
            return Ok(donors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DonorGiftsReadDTO>> GetDonorByIdAsync(int id)
        {
            var donor = await _donorService.GetDonorByIdAsync(id);
            if (donor == null)
            {
                return NotFound();
            }
            return Ok(donor);
        }

        [HttpPost]
        public async Task<ActionResult<DonorReadDTO>> CreateDonorAsync(DonorCreateDTO donorCreateDTO)
        {
            var createdDonor = await _donorService.CreateDonorAsync(donorCreateDTO);

            // במקום CreatedAtAction, נשתמש ב-Created ונעביר נתיב ידני או פשוט נחזיר Ok
            // return Created($"/api/donors/{createdDonor.Id}", createdDonor);
            return Ok(createdDonor);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DonorReadDTO>> UpdateDonorAsync(int id, DonorUpdateDTO donorUpdateDto)
        {
            var updatedDonor = await _donorService.UpdateDonorAsync(id, donorUpdateDto);
            if (updatedDonor == null)
            {
                return NotFound();
            }
            return Ok(updatedDonor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonorAsync(int id)
        {
            var result = await _donorService.DeleteDonorAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
