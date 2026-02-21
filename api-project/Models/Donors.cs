namespace api_project.Models
{
    public class Donors
    {
        public int Id { get; set; }
        public string DonorName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<Gifts> Gifts { get; set; } = new List<Gifts>();
    }
}
