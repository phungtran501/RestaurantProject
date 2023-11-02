using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.UI.Areas.Admin.Models
{
    public class RoleViewModel
    {
        public string? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
