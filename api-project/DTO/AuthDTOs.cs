using System.ComponentModel.DataAnnotations;
using static api_project.DTO.UserDTOs;

namespace api_project.DTO;

public class LoginRequestDto
{
    [Required,EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$", ErrorMessage = "Password must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter, and one digit.")]
    public string Password { get; set; } = string.Empty;
}

public class LoginReadDto
{
    public string Token { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
    public UserReadForLoginDTO User { get; set; } = null!;
}
