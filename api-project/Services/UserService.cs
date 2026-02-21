using api_project.Interfaces;
using api_project.Models;
using api_project.DTO;
using api_project.Services;
using static api_project.DTO.UserDTOs;

namespace api_project.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;
        public UserService(ITokenService tokenService, IConfiguration configuration, ILogger<UserService> logger, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _configuration = configuration;
            _logger = logger;
            _userRepository = userRepository;
        }
        public async Task<UserReadDTO> CreateUserAsync(UserCreateDTO userCreateDTO)
        {
            if (await _userRepository.EmailExistsAsync(userCreateDTO.Email))
            {
                throw new ArgumentException("Email already exists");
            }

            var user = new User
            {
                Name = userCreateDTO.Name,
                Email = userCreateDTO.Email,
                Phone = userCreateDTO.Phone,
                Password = HashPassword(userCreateDTO.Password)
            };
            var createdUser = await _userRepository.CreateAsync(user);
            _logger.LogInformation($"User created with ID: {createdUser.Id}");
            var userReadDTO = new UserReadDTO
            {
                Id = createdUser.Id,
                Name = createdUser.Name,
                Email = createdUser.Email,
                Phone = createdUser.Phone
            };
            return userReadDTO;
        }
        public async Task<LoginReadDto?> AuthenticateAsync(LoginRequestDto loginRequestDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginRequestDto.Email);
            if (user == null || user.Password != HashPassword(loginRequestDto.Password))
            {
                _logger.LogWarning($"Authentication failed for email: {loginRequestDto.Email}");
                return null;
            }
            var token = _tokenService.GenerateToken(user.Id, user.Email, user.Name,user.Role);
            var expiresIn = int.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60") * 60;
            var loginReadDto = new LoginReadDto
            {
                Token = token,

                ExpiresIn = expiresIn,
                User = new UserReadForLoginDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    Role=user.Role,
                }
            };
            _logger.LogInformation($"User authenticated with ID: {user.Id}");
            return loginReadDto;
        }
        private static string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
