namespace api_project.Models
{
    public class Gifts
    {
        public int Id { get; set; }
        public string GiftName { get; set; } = string.Empty;
        public string Description { get; set; }= string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public int NumOfPurchases { get; set; }
        public decimal Price { get; set; }
        public int DonorId { get; set; }
        public  Donors? donors { get; set; }    
        public int ?UserId { get; set; }
        public Users? winner { get; set; }
        public int CategoryName { get; set; }
        public Categories? category { get; set; }

    }
}
