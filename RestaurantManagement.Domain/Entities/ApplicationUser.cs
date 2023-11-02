using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Entities
{
    public class ApplicationUser: IdentityUser
    {
        [StringLength(250)]
        public string? Fullname { get; set; }
        [StringLength(500)]
        public string? Address { get; set; }

        public bool IsSystem { get; set; }
        public bool IsActive { get; set; }
    }
}
