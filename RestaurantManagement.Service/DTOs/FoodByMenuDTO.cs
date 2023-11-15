namespace RestaurantManagement.Service.DTOs
{
    public class FoodByMenuDTO
    {
        public string MenuName { get; set; }
        public List<MenuFood> Foods { get; set; }
    }

    public class MenuFood
    {
        public string FoodName { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }

    }
}
