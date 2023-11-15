using RestaurantManagement.Service.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Service.DTOs.Cart
{
    public class CartCheckOutDTO
    {
        public string UserId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fullname { get; set; }
        public string? Note { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
