namespace RestaurantManagement.Service.DTOs
{
    public class FoodDTO
    {
        public int Id { get; set; }

        public string FoodName { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }
        public int Available { get; set; }
        
        public bool IsActive { get; set; }

        public string CategoryName { get; set; }
        public string Code { get; set; }
    }
}
