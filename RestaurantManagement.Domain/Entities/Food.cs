using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Entities
{
    public class Food : BaseEntity
    {
        [StringLength(500)]
        public string Name { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
        [StringLength(500)]

        public int Available { get; set; }
        public double Price { get; set; }
        public  bool IsActive { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
        [StringLength(200)]
        public string Code { get; set; }
    }
}
