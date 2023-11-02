using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Entities
{
    public class Category: BaseEntity
    {
        [StringLength(500)]
        public string Name { get; set; }
        public  bool IsActive { get; set; }
    }
}
