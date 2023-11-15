
namespace RestaurantManagement.UI.Areas.Admin.Models
{
    public class MenuDetailModel
    {
        public int Id { get; set; }
        public double Price { get; set; }

        public int MenuId { get; set; }

        public int FoodId { get; set; }

    }
}
