namespace RestaurantManagement.Service.DTOs.Cart
{
    public class CartItemDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string? Code { get; set; }
        public int Quantity { get; set; }
    }
}
