using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Entities
{
    public class UserToken: BaseEntity
    {
        public int UserId { get; set; }
        public string AccessToken { get; set; }

        public DateTime ExpiredDateAccessToken { get; set; }

        [StringLength(50)]
        public string CodeRefreshToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpiredDateRefreshToken { get; set; }

        public DateTime CreatedToken { get; set; }
    }
}
