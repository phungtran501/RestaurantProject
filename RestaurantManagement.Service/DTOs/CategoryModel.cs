using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.UI.Areas.Admin.Models
{
    public class CategoryModel
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public IFormFile? CategoryImage { get; set; }
    }
}
