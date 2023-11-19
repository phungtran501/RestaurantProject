namespace RestaurantManagement.Service.DTOs.Cart
{
    public class CartCheckOutDTO
    {
        public string UserId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fullname { get; set; }
        public string? Note { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
