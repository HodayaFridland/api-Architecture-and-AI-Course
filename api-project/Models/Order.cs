namespace api_project.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int GiftId { get; set; }
        public Gift? Gift { get; set; }
        public bool IsConfirmed { get; set; } = false;
    }
}
