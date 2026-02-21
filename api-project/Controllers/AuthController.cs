using api_project.Interfaces;
using Microsoft.AspNetCore.Mvc;
using api_project.DTO;
using static api_project.DTO.UserDTOs;

namespace api_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserReadDTO>> Register([FromBody] UserCreateDTO userCreateDTO)
        {
            try
            {
                var createdUser = await _userService.CreateUserAsync(userCreateDTO);
                return CreatedAtAction(nameof(Register), new { id = createdUser.Id }, createdUser);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginReadDto>> Login([FromBody] LoginRequestDto LoginRequestDto)
        {
            if (string.IsNullOrWhiteSpace(LoginRequestDto.Email) || string.IsNullOrWhiteSpace(LoginRequestDto.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }
            var loginResult = await _userService.AuthenticateAsync(LoginRequestDto);
            if (loginResult == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }
            return Ok(loginResult);
        }
    }
}
