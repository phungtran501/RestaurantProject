namespace RestaurantManagement.Service.DTOs.Cart
{
    public class CartDetailDTO
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int FoodId { get; set; }
    }
}
