using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Service.DTOs
{
    public class ResponseDatatable
    {
        public object[] Data { get; set; }
        public int Draw { get; set; }
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
    }
}
