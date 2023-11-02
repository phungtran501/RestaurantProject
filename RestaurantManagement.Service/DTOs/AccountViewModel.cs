using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.UI.Areas.Admin.Models
{
    public class AccountViewModel
    {

        public string? Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Fullname { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public IFormFile? Avatar { get; set;}
        [Required]
        public string RoleId { get; set;}
    }
}
