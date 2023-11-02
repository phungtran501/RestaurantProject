using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.Domain.Entities
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [System.ComponentModel.DataAnnotations.Required]
        public int Id { get; set; }
    }
}
