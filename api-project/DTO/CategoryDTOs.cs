using System.ComponentModel.DataAnnotations;

namespace api_project.DTO
{
    public class CategoryDTOs
    {
        public class CategoryCreateDTO
        {
            [Required]
            public string Name { get; set; } = string.Empty;
        }
    }
}
