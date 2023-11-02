using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Entities
{
    public class UserAddress: BaseEntity
    {
        [StringLength(500)]
        public string Address { get; set; }
        [StringLength(15)]
        public string Phone { get; set; }

        [StringLength(250)]
        public string Fullname { get; set;}
        public bool IsActive { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
