using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.Domain.Entities
{
    public class Cart: BaseEntity
    {
        [StringLength(500)]
        public string? Note { get; set; }
        public DateTime CreateDate { get; set; }
        public short Status { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
