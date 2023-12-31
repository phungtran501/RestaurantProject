﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.Domain.Entities
{
    public class CartDetail: BaseEntity
    {
        public double Price { get; set; }
        public int Quantity { get; set; }
        [StringLength(1000)]

        public string? Note { get; set; }

        public int CartId { get; set; }
        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; }

        public int FoodId { get; set; }
        [ForeignKey(nameof(FoodId))]
        public Food Food { get; set; }
    }
}
