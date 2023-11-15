using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Service.DTOs.Cart
{
    public class CartDetailModel
    {
        public int Id { get; set; }
        public double FoodPrice { get; set; }
        public int Quantity { get; set; }
        public string FoodName { get; set; }

        public double TotalPrice { get; set; }
    }
}
