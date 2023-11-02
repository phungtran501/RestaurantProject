using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Entities
{
    public class MenuDetail: BaseEntity
    {
     
        public double Price { get; set; }

        public int MenuId { get; set; }
        [ForeignKey(nameof(MenuId))]
        public Menu Menu { get; set; }

        public int FoodId { get; set; }
        [ForeignKey(nameof(FoodId))]
        public Food Food { get; set; }
    }
}
