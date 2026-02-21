using System.ComponentModel.DataAnnotations;
using api_project.Models;

namespace api_project.DTO
{
    public class OrderCreateDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int GiftId { get; set; }

    }
    public class OrderReadDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int GiftId { get; set; }
        public bool IsConfirmed { get; set; }
    }
    public class OrderUpdateDto {
        [Required]
        public bool IsConfirmed { get; set; }
    }
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GiftId { get; set; }
        public GiftReadDTO ?Gift { get; set; }
        public bool IsConfirmed { get; set; }
    }

}
