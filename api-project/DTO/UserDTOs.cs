using System.ComponentModel.DataAnnotations;
using api_project.Models;

namespace api_project.DTO
{
    public class UserDTOs
    {
        public class UserCreateDTO
        {
            [Required,MaxLength(20)]
            public string Name { get; set; } = string.Empty;
            [Required, EmailAddress]
            public string Email { get; set; } = string.Empty;
            
            public string? Phone { get; set; } = string.Empty;

            [Required, RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$", ErrorMessage = "Password must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter, and one digit.")]
            public string Password { get; set; } = string.Empty;

        }
        public class UserUpdateDTO
        {
            [Required, MinLength(20)]
            public string Name { get; set; } = string.Empty;
            [Phone]
            public string Phone { get; set; } = string.Empty;
        }
        public class UserReadDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
        }
        public class UserReadForLoginDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
        }

        public class UserOrdersReadDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public string password { get; set; } = string.Empty;
            public ICollection<OrderReadDto> Orders { get; set; } = new List<OrderReadDto>();
        }

    }

    
}
