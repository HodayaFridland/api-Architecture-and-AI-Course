namespace api_project.Models
{
    public class Gift
    {
        public int Id { get; set; }
        public string GiftName { get; set; } = string.Empty;
        public string Description { get; set; }= string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public int NumOfPurchases { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int DonorId { get; set; }
        public  Donor? Donor { get; set; }    
        public int ?WinnerId { get; set; }
        public User? Winner { get; set; }
       

    }
}
