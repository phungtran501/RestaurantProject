using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.UI.Areas.Admin.Models
{
    public class MenuViewModel
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
