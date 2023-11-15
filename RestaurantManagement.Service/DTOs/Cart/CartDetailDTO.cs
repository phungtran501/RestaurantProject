using RestaurantManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Service.DTOs.Cart
{
    public class CartDetailDTO
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int FoodId { get; set; }
    }
}
