namespace api_project.Models
{
    public class Donor
    {
        public int Id { get; set; }
        public string DonorName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<Gift> Gifts { get; set; } = new List<Gift>();
    }
}
