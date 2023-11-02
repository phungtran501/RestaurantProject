using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using System.Xml.Linq;

namespace RestaurantManagement.UI.Areas.Admin.Models
{
    public class LoginModel
    {

        //[Required(AllowEmptyStrings =true)]
        //public string ReturnUrl { get; set; }


        //public InputModel Input { get; set; }

        [Required(ErrorMessage = "Your Email or Username must be not empty")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Your Password must be not empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }



    }
}
