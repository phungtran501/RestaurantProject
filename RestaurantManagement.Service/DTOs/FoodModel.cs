using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RestaurantManagement.UI.Areas.Admin.Models
{
    public class FoodModel
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile? Image { get; set; }

        public int Available { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string Code { get; set; }

    }
}
