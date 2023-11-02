using RestaurantManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.UI.Areas.Admin.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
        public int FoodId { get; set; }

        public string UserName { get; set; }
        public bool IsActive { get; set; }
    }
}
