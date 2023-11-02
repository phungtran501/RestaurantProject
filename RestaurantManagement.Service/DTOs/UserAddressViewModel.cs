using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.UI.Areas.Admin.Models
{
    public class UserAddressViewModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public string Fullname { get; set; }
        public bool IsActive { get; set; }

        public string UserId { get; set; }
    }
}
