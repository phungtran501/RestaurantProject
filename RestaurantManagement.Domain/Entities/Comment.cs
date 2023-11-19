using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.Domain.Entities
{
    public class Comment: BaseEntity
    {
        [StringLength(500)]
        public string? Content { get; set; }

        public DateTime CreatedOn { get; set; }
        public int FoodId { get; set; }
        [ForeignKey(nameof(FoodId))]
        public Food Food { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; }

        public bool IsActive { get; set; }
    }
}
