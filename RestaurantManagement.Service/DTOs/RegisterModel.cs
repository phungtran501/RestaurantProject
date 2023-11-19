using System.ComponentModel.DataAnnotations;


namespace RestaurantManagement.UI.Areas.Admin.Models
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Fullname { get; set; }
    }
}
