using System.ComponentModel.DataAnnotations;
using static api_project.DTO.UserDTOs;

namespace api_project.DTO
{
    public class GiftCreateDTO()
    {
        [Required]
        public string GiftName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string CategoryName { get; set; }= string.Empty; 
        public int DonorId { get; set; }
   

    }
    public class GiftUpdateDto()
    {
        [Required]
        public string GiftName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        public string ?CategoryName { get; set; } = string.Empty;
        public int? WinnerId { get; set; }

    }

    public class GiftReadDTO() {
            public int Id { get; set; }
            public string GiftName { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string ? ImageUrl { get; set; } 
            public string? CategoryName { get; set; }
           public string? Descriptoin { get; set; }
            public int ? DonorId { get; set; }
    }
    public class GiftReadWithWinnerDTO()
    {
        public int Id { get; set; }
        public string GiftName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public UserReadDTO ?Winner { get; set; }
    }
}
