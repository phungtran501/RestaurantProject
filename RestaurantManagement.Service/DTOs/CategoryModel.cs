using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


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
