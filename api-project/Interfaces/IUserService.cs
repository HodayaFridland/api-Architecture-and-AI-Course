using api_project.DTO;
using api_project.DTO;

namespace api_project.Interfaces
{
    public interface IUserService
    {
        Task<LoginReadDto?> AuthenticateAsync(LoginRequestDto loginRequestDto);
        Task<UserDTOs.UserReadDTO> CreateUserAsync(UserDTOs.UserCreateDTO userCreateDTO);
    }
}