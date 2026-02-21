namespace api_project.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Users? User { get; set; }
        public int GiftId { get; set; }
        public Gifts? gift { get; set; }
        public bool IsConfirmed { get; set; } = false;
    }
}
