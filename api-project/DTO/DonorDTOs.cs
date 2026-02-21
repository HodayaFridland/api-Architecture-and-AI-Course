using System.ComponentModel.DataAnnotations;
using api_project.Models;


namespace api_project.DTO
{
   
        public class DonorCreateDTO
        {
           
            [Required,MaxLength(100), MinLength(2)]
        public string DonorName { get; set; } = string.Empty;
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
        }
        public class DonorUpdateDTO
        {
        [Required, MaxLength(100), MinLength(2)]
        public string DonorName { get; set; } = string.Empty;
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
        }
        public class DonorReadDTO
        {
            public int Id { get; set; }
            public string DonorName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
        }
        public class DonorGiftsReadDTO
        {
            public int Id { get; set; }
            public string DonorName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public ICollection<GiftReadDTO> Gifts { get; set; } = new List<GiftReadDTO>();
        }
        
             
    }

