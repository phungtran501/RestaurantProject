namespace RestaurantManagement.Service.DTOs.Cart
{
    public class CartDTO
    {
        public int Id { get; set; }
        public string? Note { get; set; }
        public DateTime CreateDate { get; set; }
        public short Status { get; set; }

        public string Username { get; set; }
    }
}
