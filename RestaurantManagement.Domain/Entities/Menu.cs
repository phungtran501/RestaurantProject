using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Domain.Entities
{
    public class Menu: BaseEntity
    {
        [StringLength(500)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
